
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
import Loader from '../../UserControls/Loader/Loader';
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
    const { setTitle, setBanner } = useOutletContext<LayoutContext>();
    const location = useLocation();

    const [recipe, setRecipe] = useState<RecipeView | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [auth, setAuth] = useState<AuthResult | null>(null);
    const [measurementSystem, setMeasurementSystem] = useState<"Imperial" | "Metric" | null>(null);

    const [fontClass, setFontClass] = useState<string | null>(null);
    const [backgroundCardClass, setBackgroundCardClass] = useState<string | null>(null);


    const layoutClass = isMobileTouchDevice() ? "gof-view-mobile" : "gof-tall";
    const innerlayoutClass = isMobileTouchDevice() ? "grid-page-row-height-mobile" : "grid-page-row-height-desktop";
    
    //const backgroundCardClass = "hello";

    //const layoutClass = "";
    //const innerlayoutClass = "";

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
        if (auth) {

            const value = auth.claims?.MeasurementSystem;

            if (value === "Imperial") {
                setMeasurementSystem("Imperial");
            } else if (value === "Metric") {
                setMeasurementSystem("Metric");
            } else {
                setMeasurementSystem(null);
            }
            //console.log("Measurement System: " + measurementSystem);
        }
    }, [auth]);


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
    console.log("Font class: " + fontClass);

    const handlePrint = () => {
        window.print();
    }


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

                                    {recipe?.isMyRecipe && (
                                        <div className="d-print-none">
                                            <button
                                                type="button"
                                                className="button button-icon"
                                                onClick={() => navigate(`/recipes/ingredients/${recipe.id}`)}
                                            >
                                                <Icon name="pencil" />
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

                                    {recipe?.isMyRecipe && (
                                        <div className="d-print-none">
                                            <button
                                                type="button"
                                                className="button button-icon"
                                                onClick={() => navigate(`/recipes/steps/${recipe.id}`)}
                                            >
                                                <Icon name="pencil" />
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
            <ButtonGrid
                buttons={[
                    {
                        text: "My Recipes",
                        url: "/recipes/myrecipes",
                        type: "button",
                        mobileSlot: 2,
                        desktopSlot: 3
                    },
                    {
                        text: "Print",
                        onClick: handlePrint,
                        type: "button",
                        icon: <Icon name="print" />,
                        mobileSlot: 3,
                        desktopSlot: 5
                    }
                ]}
            />
        </>
    );
}

export default View;