import axios from 'axios';
import React, { useState, useEffect, useMemo } from 'react';
import { useOutletContext, useParams, useNavigate, useLocation } from 'react-router-dom';
import { getApiBaseUrl } from '../../../helpers/config';
import CheckAuth from '../../../components/Account/CheckAuth';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';
import type { LayoutContext } from '../../Layout';
import Icon from '../../UserControls/Icons/icons';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import ProgressBar from '../../UserControls/ProgressBar/ProgressBar';
import Loader from '../../UserControls/Loader/Loader';
import '../../../radio.css';

const API_BASE = getApiBaseUrl();

interface RecipeForm {
    name: string;
    description: string;
    showAbbreviations: boolean;
    recipeVisibility: "MeOnly" | "AllUsers" | null;
}

interface RecipeValidationErrors {
    recipe?: {
        name?: string;
        description?: string;
        showAbbreviations?: string;
        recipeVisibility?: string;

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
    const { setTitle, setBanner } = useOutletContext<LayoutContext>();
    const location = useLocation();

    useEffect(() => {

        return () => {

            setBanner('');

        };
    }, []);

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
        recipeVisibility: 'MeOnly'

    });
    const [errors, setErrors] = useState<{ recipe: Partial<Record<keyof RecipeForm, string>> }>({ recipe: {} });
    const [selectedCategory, setSelectedCategory] = useState<{ id: string; text: string } | null>(null);




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
                    console.warn('Unauthorized access attempt');
                    navigate('/dashboard');
                    return;
                }

                const hydratedRecipe: RecipeForm = {
                    name: data.name,
                    description: data.description,
                    showAbbreviations: data.showAbbreviations,
                    recipeVisibility: data.recipeVisibility

                };

                setRecipe(hydratedRecipe);


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
                RecipeVisibility: formData.recipeVisibility
            };
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
                RecipeVisibility: formData.recipeVisibility
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

    if (auth === null) return <div><Loader message="Loading recipe info ..." /></div>;
    if (!auth.auth) return null;


    return (
        <>

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
                                        <span id="layoutPreferenceText" className="ps-3 d-block"></span>
                                    </div>

                                </div>
                                <span className="error-message-placeholder-height">&nbsp;</span>
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
                        </div>
                    </div>
                </div>
            </form>

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
