
import React, { useState, useEffect, useMemo } from 'react';
import { useOutletContext, useParams, useNavigate, useLocation } from 'react-router-dom';
import { getApiBaseUrl, isMobileTouchDevice } from '../../../helpers/config';
import { renderNumberDisplayBySystem, renderStep, renderUnit } from '../../../helpers/measurementHelper';
import { mapFontToClass } from '../../../helpers/displayHelper';
import CheckAuth from '../../../components/Account/CheckAuth';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';
import type { LayoutContext } from '../../Layout';
import Icon from '../../UserControls/Icons/icons';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import type { RecipeView } from '../../../types/Recipe/Recipe';
import type { PreviousPageNavigation } from '../../../types/Navigation/Navigation';
import Loader from '../../UserControls/Loader/Loader';
import FavoritesStar from '../../../components/UserControls/Favorites/FavoriteStar';
import '../../../grid-layout.css';
import './view.css';

const API_BASE = getApiBaseUrl();


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

function View() {
    const { id } = useParams<{ id?: string }>();

    const navigate = useNavigate();
    const { setTitle, setBanner, setTitleBarSlot, previousPath } = useOutletContext<LayoutContext>();
    const location = useLocation();

    //const [restoredSearchTerm, setRestoredSearchTerm] = useState<string | null>(location.state?.searchTerm);
    const [restoredSearchTerm, setRestoredSearchTerm] = useState<string | null>(null);

    const [recipe, setRecipe] = useState<RecipeView | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [auth, setAuth] = useState<AuthResult | null>(null);
    const [measurementSystem, setMeasurementSystem] = useState<"Imperial" | "Metric" | null>(null);

    const [fontClass, setFontClass] = useState<string | null>(null);
    const [backgroundCardClass, setBackgroundCardClass] = useState<string | null>(null);
    const [previousPageNavigation, setPreviousPathNavigation] = useState<PreviousPageNavigation | null>(null);
    const [isMyRecipe, setIsMyRecipe] = useState<boolean>(false);


    const layoutClass = isMobileTouchDevice() ? "gof-view-mobile" : "gof-tall";
    const innerlayoutClass = isMobileTouchDevice() ? "grid-page-row-height-mobile" : "grid-page-row-height-desktop";
    const isMobile = isMobileTouchDevice();
    //console.log("Previous location:", previousPath.current);

    //Set previous page navigation button correctly
    useEffect(() => {
        const prev = previousPath.current;
        if (!prev) return;

        const lower = prev.toLowerCase();
        let buttonText = "";
        let url = prev;

        // Special case: Steps should behave like MyRecipes
        if (lower.includes("steps")) {
            buttonText = "My Recipes";
            url = "/recipes/myrecipes";
        }
        else if (lower.includes("myrecipes")) {
            buttonText = "My Recipes";
        }
        else if (lower.includes("favorites")) {
            buttonText = "Favorites";
        }
        else if (lower.includes("search")) {
            buttonText = "Back to Search";
            if (restoredSearchTerm == null || restoredSearchTerm == "")
                setRestoredSearchTerm("<ALL/>");

        }
        else {
            buttonText = "Back";
        }

        setPreviousPathNavigation({ buttonText, url });

    }, [previousPath.current]);

    //console.log("Search: " + restoredSearchTerm);
    useEffect(() => {
        if (location.state?.searchTerm !== undefined) {
            setRestoredSearchTerm(location.state.searchTerm);
        }
    }, []);




    useEffect(() => {
        CheckAuth().then(setAuth);
    }, []);

    useEffect(() => {
        if (!auth) return;

        if (!auth.auth) {
            navigate('/signin');
            return;
        }


    }, [auth, navigate]);

    useEffect(() => {
        return () => {
            setBanner('');
            setTitleBarSlot(null);
        };
    }, [setTitleBarSlot]);

    useEffect(() => {
        setTitleBarSlot(<FavoritesStar recipeId={id} />);
    }, [id]);

    useEffect(() => {
        const load = async () => {
            const r = await checkIsMyRecipe(id);
            setIsMyRecipe(r);
        };

        if (id) {
            load();
        }
    }, [id]);



    const checkIsMyRecipe = async (recipeId: string): Promise<boolean> => {
        const res = await fetch(`${API_BASE}/api/View/isMyRecipe/${recipeId}`, {
            method: "GET",
            credentials: "include"
        });

        if (!res.ok) {
            console.error("Failed to check recipe ownership");
            return false;
        }

        const data = await res.json();
        return data.isOwner === true;
    };



    useEffect(() => {
        async function load() {
            try {
                const res = await fetch(`${API_BASE}/api/View/${id}`, {
                    credentials: "include"
                });

                if (!res.ok) {
                    throw new Error(`Failed to load recipe ${id}`);
                }

                const data: RecipeView = await res.json();

                //need to write logic for this later when
                //implementing search other's recipes
                data.isMyRecipe = true;
                //console.log("Is My Recipe: " + data.isMyRecipe);
                //console.log("Recipe data: " + JSON.stringify(data));
                //console.log("Font: " + data.recipeFont);
                setRecipe(data);

                const value = data.measurementSystem;

                if (value === "Imperial") {
                    setMeasurementSystem("Imperial");
                } else if (value === "Metric") {
                    setMeasurementSystem("Metric");
                } else {
                    setMeasurementSystem(null);
                }
                //console.log("Measurement System: " + data.measurementSystem);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        load();
    }, [id]);

    //Set title
    useEffect(() => {
        if (!loading && recipe?.name) {
            setTitle(recipe.name);
        }
    }, [recipe, loading, setTitle]);

    useEffect(() => {
        if (recipe?.recipeFont) {
            setFontClass(mapFontToClass(recipe.recipeFont));
            setBackgroundCardClass(recipe.recipeFont == "Handwritten" ? "recipe-card-handwritten-font" : "hello");
        }
    }, [recipe?.recipeFont]);


    //const fontClass = getFontClass(recipe?.recipeFont);
    //console.log("Font class: " + fontClass);

    const handlePrint = () => {
        window.print();
    }

    const buttonGridConfig = useMemo(() => [
        {
            text: previousPageNavigation?.buttonText,
            url: previousPageNavigation?.url,
            type: "button" as const,
            mobileSlot: 2,
            desktopSlot: 3,
            navigateOptions: {
                state: { searchTerm: restoredSearchTerm }
            }
        },
        {
            text: "Print",
            onClick: handlePrint,
            type: "button" as const,
            icon: <Icon name="print" />,
            mobileSlot: 3,
            desktopSlot: 5
        }
    ], [previousPageNavigation, restoredSearchTerm]);




    if (measurementSystem === null || fontClass === null) return <div><Loader message="Loading recipe ..." /></div>;

    if (auth === null) return <div><Loader message="Loading recipe ..." /></div>;
    if (!auth.auth) return null;


    return (
        <>


            <div className="content-holder-desktop" >
                {/* <div className="content-inner-desktop "> */}
                <div className={`content-inner-desktop ${backgroundCardClass}`}>

                    {/*INSERT CONTENT HERE */}

                    <div className={`pt-4 grid-overflow-box gof-row  ${layoutClass} ${fontClass} `}>
                        <div className={`d-flex row align-items-start ${innerlayoutClass}`}>
                            <div className="col-md-6 col-12 pt-2">
                                <div className="d-flex align-items-center pb-3 fw-bold">
                                    <span className="me-2">Ingredients</span>

                                    {isMyRecipe && (
                                        <div className="d-print-none">
                                            <button
                                                type="button"
                                                className="button button-icon"
                                                onClick={() => navigate(`/recipes/ingredients/${recipe.id}`)}
                                            >
                                                {isMobile ? (
                                                    <Icon name="pencil" marginLeft={4} />
                                                ) : (
                                                    <Icon name="pencil" marginLeft={1} marginTop={-2} />
                                                )}
                                            </button>
                                        </div>
                                    )}
                                </div>

                                {/* Forces vertical block flow and resets padding for bullet indents */}

                                {recipe?.ingredients && recipe.ingredients.length > 0 ? (
                                    <ul
                                        style={{ display: "block", paddingLeft: "20px", listStyleType: "disc" }}
                                        className="m-0"
                                    >
                                        {recipe.ingredients
                                            .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
                                            .map((ingredient) => (
                                                <li
                                                    key={ingredient.id}
                                                    style={{ display: "list-item", width: "100%" }}
                                                    className="pb-2 text-specified"
                                                >
                                                    <span
                                                        dangerouslySetInnerHTML={{
                                                            __html: renderNumberDisplayBySystem(
                                                                ingredient.quantity.toString(),
                                                                measurementSystem,
                                                                recipe.recipeFont
                                                            ),
                                                        }}
                                                    />
                                                    &nbsp;{renderUnit(ingredient.unit)}{ingredient.description}
                                                </li>
                                            ))}
                                    </ul>
                                ) : (
                                    <div className="text-start">
                                        No ingredients found.  Start adding some.
                                    </div>
                                )}


                            </div>

                            {/* Steps Column */}
                            <div className="col-md-6 col-12 pt-2">
                                <div className="d-flex align-items-center pb-2 fw-bold">
                                    <span className="me-2">Steps</span>

                                    {isMyRecipe && (
                                        <div className="d-print-none">
                                            <button
                                                type="button"
                                                className="button button-icon"
                                                onClick={() => navigate(`/recipes/steps/${recipe.id}`)}
                                            >
                                                {isMobile ? (
                                                    <Icon name="pencil" marginLeft={4} />
                                                ) : (
                                                    <Icon name="pencil" marginLeft={1} marginTop={-2} />
                                                )}
                                            </button>
                                        </div>
                                    )}
                                </div>

                                {recipe?.steps && recipe.steps.length > 0 ? (
                                    <ol>
                                        {recipe.steps
                                            .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
                                            .map((step) => (
                                                <li
                                                    key={step.id}
                                                    className="pb-2 text-specified"
                                                    dangerouslySetInnerHTML={{
                                                        __html: renderStep(step.description, recipe.recipeFont)
                                                    }}
                                                />
                                            ))}
                                    </ol>
                                ) : (
                                    <div className="text-start">
                                        No steps found. Start adding some.
                                    </div>
                                )}


                            </div>
                        </div>
                    </div>




                </div>
            </div>

            {/* BUTTON SLOT FEATURE */}
            <ButtonGrid buttons={buttonGridConfig} />

        </>
    );
}

export default View;