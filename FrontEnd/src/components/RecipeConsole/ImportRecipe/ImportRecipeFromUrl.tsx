import axios from 'axios';
import React, { useState, useEffect, useMemo } from 'react';
import { useOutletContext, useParams, useNavigate, useLocation } from 'react-router-dom';
import { getApiBaseUrl } from '../../../helpers/config';
import CheckAuth from '../../Account/CheckAuth';
import type { LayoutContext } from '../../Layout';
import Icon from '../../UserControls/Icons/icons';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import Loader from '../../UserControls/Loader/Loader';


const API_BASE = getApiBaseUrl();

interface RecipeForm {
    url: string;
}

interface RecipeValidationErrors {
    recipe?: {
        url?: string;
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

const ImportRecipeFromUrl: React.FC = () => {
    const { id } = useParams<{ id?: string }>();
    const isEditMode = !!id;

    const navigate = useNavigate();
    const { setTitle, setBanner, setTitleBarSlot } = useOutletContext<LayoutContext>();
    const location = useLocation();

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
        if (location.state?.banner) {
            setBanner(location.state.banner);
            navigate(location.pathname, {
                replace: true,
                state: {},
            });
        }
    }, [location.state?.banner, setBanner, navigate, location.pathname]);




    const [auth, setAuth] = useState<AuthResult | null>(null);

    const [recipe, setRecipe] = useState<RecipeForm>({ url: '' });

    const [errors, setErrors] = useState<{ recipe: Partial<Record<keyof RecipeForm, string>> }>({ recipe: {} });

    const [isImporting, setIsImporting] = useState(false);

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


    //Set title
    useEffect(() => {
        setTitle('Import Recipe from Url');
    }, [setTitle]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setRecipe(prev => ({ ...prev, [name]: value }));
    };

    const handleImport = async () => {
        setBanner('');

        // --- VALIDATION: URL cannot be blank ---
        if (!recipe.url || recipe.url.trim() === "") {
            setErrors({
                recipe: {
                    url: "Please enter a recipe URL."
                }
            });
            return;
        }

        setIsImporting(true);

        // Clear previous errors if any
        setErrors({ recipe: {} });

        try {
            const response = await axios.post(
                `${API_BASE}/api/recipes/import/scrapeUrl`,
                { url: recipe.url },
                { withCredentials: true }
            );

            // --- SUCCESS (200) ---
            if (response.status === 200) {
                // TODO: implement success logic
                //console.log("SUCCESS", response.data);
                navigate(`/Recipes/RecipeInfo/${response.data.recipeId}`, {
                    state: {
                        banner: "Recipe imported successfully!",
                        showImportReminder: true
                    }
                });

                return;
            }

        } catch (error: any) {

            // --- BAD REQUEST (400) ---
            if (error.response?.status === 400) {
                // TODO: implement bad request logic
                //console.log("BAD REQUEST", error.response.data);
                setBanner('Error importing recipe');
                setIsImporting(false);
                return;
            }

            // --- NOT FOUND (404) ---
            if (error.response?.status === 404) {
                // TODO: implement not found logic
                //console.log("NOT FOUND", error.response.data);
                setBanner('No recipe found at Url');
                setIsImporting(false);
                return;
            }

            // Optional: other errors
            setBanner('Error importing recipe');
            setIsImporting(false);
            //console.log("UNEXPECTED ERROR", error);
        }
    };


    if (auth === null) return <div><Loader message="Loading recipe info ..." /></div>;
    if (!auth.auth) return null;

    if (isImporting) {
        return (<><Loader message="Importing recipe ..." /></>);
    }
    return (
        <>

            <form id="page-form">
                <div className="content-holder-desktop" >

                    <div className="content-inner-desktop">
                        <div className="page-container row pt-3">

                            <div className="page-item col-12 ">

                                {/* Row 1: Label + * */}
                                <div className="d-flex align-items-center mb-1">
                                    <label className="form-label-tight mb-0">Recipe Url</label>
                                    <span className="required ms-1">*</span>
                                </div>

                                {/* Row 2: Textbox + Clear Button */}
                                <div className="d-flex pb-3">
                                    <input
                                        type="text"
                                        name="url"
                                        className="form-control textbox textbox-text textbox-large"
                                        value={recipe.url}
                                        onChange={handleChange}
                                    />

                                    <div className="ps-3">
                                        <button
                                            type="button"
                                            className="button button-icon button-icon-search"
                                            onClick={() => setRecipe({ url: "" })}
                                        >
                                            <span className="btn-text">
                                                <div className="margin-3"><Icon name="clear" /></div>
                                            </span>
                                        </button>
                                    </div>
                                </div>

                                {/* Error message */}
                                {errors.recipe?.url ? (
                                    <div className="error-message">{errors.recipe.url}</div>
                                ) : (
                                    <div className="error-message-placeholder-height"></div>
                                )}
                            </div>
                            <div className="page-item col-12 ">
                                Websites format recipes differently, so imports aren’t always perfect.
                                You’ll be able to review and fix anything after import.
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
                        text: "Import",
                        onClick: handleImport,
                        type: "button",
                        mobileSlot: 3,
                        desktopSlot: 5
                    }
                ]}
            />
        </>
    );
};

export default ImportRecipeFromUrl;
