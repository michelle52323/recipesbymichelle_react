import axios from 'axios';
import React, { useState, useEffect, useMemo } from 'react';
import { useOutletContext, useParams, useNavigate, useLocation } from 'react-router-dom';
import { getApiBaseUrl, isDevUseMockLogin } from '../../../helpers/config';
import CheckAuth from '../../../components/Account/CheckAuth';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';
import type { LayoutContext } from '../../Layout';
import Icon from '../../UserControls/Icons/icons';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import ProgressBar from '../../UserControls/ProgressBar/ProgressBar';
import Loader from '../../UserControls/Loader/Loader';
import FavoritesStar from '../../../components/UserControls/Favorites/FavoriteStar';
import { RecipeFont } from '../../../types/Recipe/Recipe';
import ImportRecipeReminder from '../ImportRecipe/ImportRecipeReminder';
import CategoriesActionsMenu from '../../UserControls/SubMenus/Categories/CategoriesActionsMenu';
import '../../../radio.css';
import './recipeInfo.css';
import CategoryAssignmentModal from '../Categories/CategoryAssignmentModal';
import type { Category } from '../../../types/Categories/Categories';

const API_BASE = getApiBaseUrl();

interface RecipeForm {
    name: string;
    description: string;
    showAbbreviations: boolean;
    recipeVisibility: "MeOnly" | "AllUsers" | null;
    recipeFont: "SansSerif" | "Serif" | "Handwritten";
    categories: Category[];
}

interface RecipeValidationErrors {
    recipe?: {
        name?: string;
        description?: string;
        showAbbreviations?: string;
        recipeVisibility?: string;
        recipeFont?: "SansSerif" | "Serif" | "Handwritten";

    };
}

interface Claims {
    FirstName?: string;
    UserId?: string;
    [key: string]: string | undefined;
}

interface AuthResult {
    auth: boolean;
    username?: string;
    claims?: Claims;
}

const RecipeInfo: React.FC = () => {
    const { id } = useParams<{ id?: string }>();
    const isEditMode = !!id;

    const navigate = useNavigate();
    const { setTitle, setBanner, setTitleBarSlot } = useOutletContext<LayoutContext>();
    const location = useLocation();

    const [assignCategoriesModalIsOpen, setAssignCategoriesModalIsOpen] = useState(false);
    const [allCategories, setAllCategories] = useState<Category[]>([]);
    const [recipeCategories, setRecipeCategories] = useState<Category[]>([]);

    useEffect(() => {

        return () => {

            setBanner('');

        };
    }, []);

    useEffect(() => {
        return () => {
            setTitleBarSlot(null);
        };
    }, [setTitleBarSlot]);

    useEffect(() => {
        if (id != null) {
            setTitleBarSlot(<FavoritesStar recipeId={id} starLocation="title" />);
        }

    }, [id]);

    useEffect(() => {
        if (location.state?.banner) {
            setBanner(location.state.banner);
            navigate(location.pathname, {
                replace: true,
                state: {},
            });
        }
    }, [location.state?.banner, setBanner, navigate, location.pathname]);






    const [auth, setAuth] = useState<AuthResult | null>(null);



    const [recipe, setRecipe] = useState<RecipeForm>({
        name: '',
        description: '',
        showAbbreviations: true,
        recipeVisibility: 'MeOnly',
        recipeFont: "SansSerif",
        categories: []

    });
    const [errors, setErrors] = useState<{ recipe: Partial<Record<keyof RecipeForm, string>> }>({ recipe: {} });
    const [selectedCategory, setSelectedCategory] = useState<{ id: string; text: string } | null>(null);
    const [showImportReminder, setShowImportReminder] = useState<boolean | null>(null);

    useEffect(() => {
        if (location.state?.showImportReminder) {
            setShowImportReminder(true);
            navigate(location.pathname, {
                replace: true,
                state: {},
            });
        }
    }, [location.state?.showImportReminder, navigate, location.pathname]);


    useEffect(() => {
        CheckAuth().then(setAuth);
    }, []);

    useEffect(() => {
        if (!auth) return;

        if (!auth.auth) {
            navigate('/signin');
            return;
        }


    }, [auth, isEditMode, navigate]);


    useEffect(() => {

        if (!id || !auth?.claims?.UserId) return;

        fetch(`${API_BASE}/api/RecipeInfo/${id}`, { credentials: 'include' })
            .then(res => res.ok ? res.json() : Promise.reject('Recipe not found'))
            .then(data => {
                if (data.userId !== auth.claims?.UserId) {
                    //console.warn('Unauthorized access attempt');
                    navigate('/dashboard');
                    return;
                }

                const hydratedRecipe: RecipeForm = {
                    name: data.name,
                    description: data.description,
                    showAbbreviations: data.showAbbreviations,
                    recipeVisibility: data.recipeVisibility,
                    recipeFont: data.recipeFont,
                    categories: data.categories

                };
                setRecipe(hydratedRecipe);
                setRecipeCategories(data.categories);


            })
            .catch(err => {
                console.error(err);
                navigate('/dashboard');
            });
    }, [id, auth?.claims?.UserId, navigate]);

    //Set title
    useEffect(() => {
        if (isEditMode) {
            setTitle(`${recipe.name}`);
        } else {
            setTitle('Create New Recipe');
        }
    }, [isEditMode, recipe.name, setTitle]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setRecipe(prev => ({ ...prev, [name]: value }));
    };

    const CreateRecipe = async (
        formData: RecipeForm,
        setErrors: React.Dispatch<React.SetStateAction<{ recipe: Partial<Record<keyof RecipeForm, string>> }>>
    ): Promise<number | null> => {
        const newErrors: Partial<Record<keyof RecipeForm, string>> = {};
        if (!formData.name?.trim()) newErrors.name = "Recipe name is required.";


        if (Object.keys(newErrors).length > 0) {
            setErrors({ recipe: newErrors });
            return null;
        }

        setErrors({ recipe: {} });



        try {
            const dto = {
                Name: formData.name,
                Description: formData.description,
                ShowAbbreviations: formData.showAbbreviations,
                RecipeVisibility: formData.recipeVisibility,
                RecipeFont: formData.recipeFont,
                Categories: recipeCategories
            };

            //console.log("DATA", dto);


            const response = await axios.post(`${API_BASE}/api/RecipeInfo/create-recipe`, dto, {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true,
            });
            return response.data.success ? response.data.recipeId : null;
        } catch (error) {
            console.error("Exception during recipe creation:", error);
            return null;
        }
    };

    const UpdateRecipe = async (
        formData: RecipeForm,
        id: string,
        setErrors: React.Dispatch<React.SetStateAction<{ recipe: Partial<Record<keyof RecipeForm, string>> }>>
    ): Promise<boolean> => {

        const newErrors: Partial<Record<keyof RecipeForm, string>> = {};

        // Same validation as CreateRecipe
        if (!formData.name?.trim()) newErrors.name = "Recipe name is required.";


        if (Object.keys(newErrors).length > 0) {
            setErrors({ recipe: newErrors });
            return false;
        }

        setErrors({ recipe: {} });

        try {
            const dto = {
                Id: Number(id),
                Name: formData.name,
                Description: formData.description,
                ShowAbbreviations: formData.showAbbreviations,
                RecipeVisibility: formData.recipeVisibility,
                RecipeFont: formData.recipeFont,
                Categories: recipeCategories
            };

            const response = await axios.put(
                `${API_BASE}/api/RecipeInfo/update-recipe/${id}`,
                dto,
                {
                    headers: { "Content-Type": "application/json" },
                    withCredentials: true,
                }
            );

            return response.data.success === true;
        } catch (error) {
            console.error("Exception during recipe update:", error);
            return false;
        }
    };

    const handleSave = async () => {
        setBanner('');
        const result = isEditMode
            ? await UpdateRecipe(recipe, id, setErrors)
            : await CreateRecipe(recipe, setErrors);


        if (typeof result === "number") {
            // navigate to the newly created recipe
            if (result) {
                navigate(`/Recipes/RecipeInfo/${result}`, {
                    state: { banner: "Recipe successfully created!" },
                });
            }
        }
        else if (typeof result === "boolean") {
            // navigate back to the same recipe using the existing id
            if (result) {
                navigate(`/Recipes/RecipeInfo/${id}`, {
                    state: { banner: "Recipe updated successfully!" },
                });
            }
        }



    };

    const handleSaveAndContinue = async () => {
        setBanner('');
        const result = isEditMode
            ? await UpdateRecipe(recipe, id, setErrors)
            : await CreateRecipe(recipe, setErrors);

        if (typeof result === "number") {
            // navigate to the newly created recipe
            if (result) {
                navigate(`/Recipes/Ingredients/${result}`, {
                    state: { banner: "Recipe created successfully!" },
                });
            }
        }
        else if (typeof result === "boolean") {
            // navigate back to the same recipe using the existing id
            if (result) {
                navigate(`/Recipes/Ingredients/${id}`, {
                    state: { banner: "Recipe updated successfully!" },
                });
            }
        }
    };

    useEffect(() => {


        getCategories("Alphabetical");
    }, []);

    const getCategories = async (sortBy: "Alphabetical" | "SortOrder" | null) => {
        const endpoint = `${API_BASE}/api/Categories/list${isDevUseMockLogin() ? "mock" : ""}?sortBy=${sortBy}`;
        const response = await fetch(endpoint, {
            method: "GET",
            credentials: "include", // ← always include credentials
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            //setCategoriesIsLoading(false);
            throw new Error("Failed to fetch categories");

        }
        else {
            const data = await response.json();   // ← THIS is the important part
            setAllCategories(data);                // ← store the actual category array
            //setCategoriesIsLoading(false);

        }

    };

    const handleCloseAssignCategories = () => {
        setAssignCategoriesModalIsOpen(false);
    };

    const handleSaveAssignedCategories = async (selectedCategoryIds: number[]) => {
        try {

            // After saving, refresh recipe categories locally:
            const updated = allCategories.filter(c => selectedCategoryIds.includes(c.id));
            setRecipeCategories(updated);

            setAssignCategoriesModalIsOpen(false);

        } catch (err) {
            console.error("Error saving categories", err);
        }
    };



    if (auth === null) return <div><Loader message="Loading recipe info ..." /></div>;
    if (!auth.auth) return null;


    return (
        <>
            {showImportReminder && (
                <ImportRecipeReminder onClose={() => setShowImportReminder(false)} />
            )}



            <form id="page-form">
                <div className="content-holder-desktop" >
                    <ProgressBar />
                    <div className="content-inner-desktop">
                        <div className="page-container row pt-3">
                            <div className="page-item col-12 col-md-6">
                                <label className='form-label-tight'>Recipe Name</label>
                                <span className="required">*</span>
                                <br />
                                <div className="form-element">
                                    <input
                                        type="text"
                                        name="name"
                                        className="form-control textbox textbox-text textbox-large"
                                        value={recipe.name}
                                        onChange={handleChange}
                                    />
                                </div>
                                {errors.recipe?.name ? (
                                    <div className="error-message">{errors.recipe.name}</div>
                                ) : (
                                    <div className="error-message-placeholder-height"></div>
                                )}
                            </div>
                            <div className="page-item col-12 col-md-6">
                                <label className='form-label-tight'>Description</label>
                                <br />
                                <div className="form-element">
                                    <textarea
                                        className="form-control textarea textbox-large textarea-text"
                                        name="description"
                                        maxLength={500}
                                        value={recipe.description}
                                        onChange={handleChange}
                                        rows={4}
                                    />
                                </div>
                                <div className="error-message-placeholder-height"></div>
                            </div>
                            <div className="page-item col-12 col-md-6 ">
                                <label>Unit Display</label>
                                <div className="form-element form-row-130">
                                    <div className="radio-holder-vertical">
                                        <ul>
                                            <li>
                                                <input
                                                    type="radio"
                                                    id="show-abbreviations-true"
                                                    name="showAbbreviations"
                                                    value="true"
                                                    checked={recipe.showAbbreviations === true}
                                                    onChange={() =>
                                                        setRecipe(prev => ({ ...prev, showAbbreviations: !prev.showAbbreviations }))
                                                    }
                                                />
                                                <div className="check"></div>
                                                <label htmlFor="show-abbreviations-true">Show Abbreviations</label>
                                            </li>

                                            <li>
                                                <input
                                                    type="radio"
                                                    id="show-abbreviations-false"
                                                    name="showAbbreviations"
                                                    value="false"
                                                    checked={recipe.showAbbreviations === false}
                                                    onChange={() =>
                                                        setRecipe(prev => ({ ...prev, showAbbreviations: !prev.showAbbreviations }))
                                                    }
                                                />
                                                <div className="check"></div>
                                                <label htmlFor="show-abbreviations-false">Show Full Unit</label>
                                            </li>
                                        </ul>
                                        <span className="ps-3 d-block">
                                            Used to determine layout preference
                                        </span>
                                    </div>
                                    <div className="form-row-extra-space"></div>
                                </div>

                            </div>
                            <div className="page-item col-12 col-md-6 ">
                                <label>Recipe Font</label>
                                <div className="form-element">
                                    <div className="radio-holder-vertical">
                                        <ul>
                                            <li>
                                                <input
                                                    type="radio"
                                                    id="recipe-font-sans-serif"
                                                    name="recipeFont"
                                                    value="SansSerif"
                                                    checked={recipe.recipeFont === "SansSerif"}
                                                    onChange={() =>
                                                        setRecipe(prev => ({
                                                            ...prev,
                                                            recipeFont: "SansSerif"
                                                        }))
                                                    }
                                                />
                                                <div className="check"></div>
                                                <label htmlFor="recipe-font-sans-serif">Sans‑Serif</label>
                                            </li>

                                            <li>
                                                <input
                                                    type="radio"
                                                    id="recipe-font-handwritten"
                                                    name="recipeFont"
                                                    value="Handwritten"
                                                    checked={recipe.recipeFont === "Handwritten"}
                                                    onChange={() =>
                                                        setRecipe(prev => ({
                                                            ...prev,
                                                            recipeFont: "Handwritten"
                                                        }))
                                                    }
                                                />
                                                <div className="check"></div>
                                                <label htmlFor="recipe-font-handwritten">Handwritten</label>
                                            </li>
                                        </ul>

                                        <span className="ps-3 d-block">
                                            Choose how this recipe will be displayed
                                        </span>
                                    </div>

                                    <div className="form-row-extra-space"></div>
                                </div>
                            </div>

                            <div className="page-item col-12 col-md-6 ">
                                <label>Recipe Visibility</label>
                                <div className="form-element">
                                    <div className="radio-holder-vertical">
                                        <ul>
                                            <li>
                                                <input
                                                    type="radio"
                                                    id="recipe-visibility-me-only"
                                                    name="recipeVisibility"
                                                    value="MeOnly"
                                                    checked={recipe.recipeVisibility === 'MeOnly'}
                                                    onChange={() => setRecipe(prev => ({ ...prev, recipeVisibility: "MeOnly" }))}

                                                />
                                                <div className="check"></div>
                                                <label htmlFor="recipe-visibility-me-only">Me Only</label>
                                            </li>

                                            <li>
                                                <input
                                                    type="radio"
                                                    id="recipe-visibility-all-users"
                                                    name="recipeVisibility"
                                                    value="AllUsers"
                                                    checked={recipe.recipeVisibility === 'AllUsers'}
                                                    onChange={() => setRecipe(prev => ({ ...prev, recipeVisibility: "AllUsers" }))}

                                                />
                                                <div className="check"></div>
                                                <label htmlFor="recipe-visibility-all-users">All Users</label>
                                            </li>
                                        </ul>
                                        <span className="ps-3 d-block">
                                            Enables recipe to be searchable by other users
                                        </span>

                                    </div>
                                    <span className="error-message-placeholder-height">&nbsp;</span>
                                </div>
                            </div>

                            <div className="page-item col-12 col-md-6">
                                <div className="row">

                                    <div className="form-element pt-3 pb-3 col-12 col-md-6">
                                        <button
                                            type="button"
                                            onClick={() => { setAssignCategoriesModalIsOpen(true); }}
                                            className="button button-super-large"
                                        >
                                            Assign to Categories
                                        </button>
                                    </div>

                                    <div className="col-12 col-md-6">
                                        <div className="categories-overflow-box panel">
                                            {(!recipeCategories || recipeCategories.length === 0) ? (
                                                <div className="no-categories">
                                                    Not assigned to any categories
                                                </div>
                                            ) : (
                                                <>
                                                    <div className="categories-header">
                                                        Assigned to these categories:
                                                    </div>

                                                    <div className="categories-list ps-3">
                                                        {recipeCategories
                                                            .slice() // ← avoid mutating state
                                                            .sort((a, b) => a.name.localeCompare(b.name))
                                                            .map(c => (
                                                                <div key={c.id} className="category-pill">
                                                                    {c.name}
                                                                </div>
                                                            ))
                                                        }

                                                    </div>
                                                </>
                                            )}
                                        </div>
                                    </div>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </form>

            {assignCategoriesModalIsOpen && (
                <CategoryAssignmentModal
                    show={assignCategoriesModalIsOpen}
                    onClose={handleCloseAssignCategories}
                    onSave={handleSaveAssignedCategories}
                    allCategories={allCategories}
                    recipeCategories={recipeCategories}
                />

            )}

            {/* BUTTON SLOT FEATURE */}
            <ButtonGrid
                buttons={[
                    {
                        text: "Cancel",
                        url: "/recipes/myrecipes",
                        type: "button",
                        mobileSlot: 1,
                        desktopSlot: 3
                    },
                    {
                        text: "Save",
                        onClick: handleSave,
                        type: "button",
                        value: "Save",
                        mobileSlot: 2,
                        desktopSlot: 4
                    },
                    {
                        text: "Next",
                        onClick: handleSaveAndContinue,
                        type: "button",
                        value: "SaveContinue",
                        icon: <Icon name="rightArrow" />,
                        mobileSlot: 3,
                        desktopSlot: 5
                    }
                ]}
            />
        </>
    );
};

export default RecipeInfo;
