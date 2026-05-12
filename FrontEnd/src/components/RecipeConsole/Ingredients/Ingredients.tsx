import { useEffect, useState, useCallback } from "react";
import { useNavigate, useLocation, useOutletContext, useParams } from "react-router-dom";

import { isMobileTouchDevice } from "../../../helpers/config";
import CheckAuth from "../../../components/Account/CheckAuth";
import type { LayoutContext } from "../../Layout";

import IngredientsListMobile from "./IngredientsMobile";
import IngredientsListDesktop from "./IngredientsDesktop";
import { getApiBaseUrl } from "../../../helpers/config";
import ProgressBar from "../../UserControls/ProgressBar/ProgressBar";

import ButtonGrid from "../../UserControls/ButtonGrid/ButtonGrid";
import Icon from "../../UserControls/Icons/icons";
import Loader from "../../UserControls/Loader/Loader";

import type { Ingredient, Unit } from "../../../types/Recipe/Recipe";
import type {
    IngredientGrid,
    IngredientGridController,
} from "../../../types/Recipe/IngredientsGrid";
import type { DragEndEvent } from "@dnd-kit/core";


const API_BASE = getApiBaseUrl();

interface RecipeForm {
    name: string;
    description: string;
}

interface AuthResult {
    auth: boolean;
    claims: Record<string, string>;
}

function Ingredients() {
    const navigate = useNavigate();
    const location = useLocation();

    const { setTitle } = useOutletContext<{ setTitle: (title: string) => void }>();
    const { setBanner } = useOutletContext<{ setBanner: (message: string) => void }>();

    const { id: recipeId } = useParams();
    if (!recipeId) navigate("/dashboard");

    const [auth, setAuth] = useState<AuthResult | null>(null);

    const [recipe, setRecipe] = useState<RecipeForm>({
        name: "",
        description: "",
    });

    // GRID STATE
    const [ingredients, setIngredients] = useState<Ingredient[]>([]);
    const [validUnits, setValidUnits] = useState<Unit[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    // DELETE MODAL STATE
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [ingredientToDelete, setIngredientToDelete] = useState<Ingredient | null>(null);
    const [ingredientIndexToDelete, setIngredientIndexToDelete] = useState<number | null>(null);

    // -----------------------------
    // AUTH + TITLE + BANNER LOGIC
    // -----------------------------

    useEffect(() => {
        return () => setBanner("");
    }, []);

    useEffect(() => {
        if (location.state?.banner) {
            setBanner(location.state.banner);
            navigate(location.pathname, { replace: true, state: {} });
        }
    }, [location.state?.banner, navigate, location.pathname, setBanner]);

    useEffect(() => {
        async function hydrateAuth() {
            const result = await CheckAuth();
            if (result && typeof result.auth === "boolean") {
                setAuth(result as AuthResult);
            } else {
                setAuth({ auth: false, claims: {} });
            }
        }
        hydrateAuth();
    }, []);

    useEffect(() => {
        if (auth === null) return;
        if (!auth.auth) navigate("/signin");
        else setTitle(recipe.name);
    }, [auth, navigate, recipe.name, setTitle]);

    // -----------------------------
    // RECIPE INFO
    // -----------------------------

    useEffect(() => {
        if (!recipeId) return;

        fetch(`${API_BASE}/api/RecipeInfo/${recipeId}`, { credentials: "include" })
            .then((res) => (res.ok ? res.json() : Promise.reject("Recipe not found")))
            .then((data) => {
                setRecipe({
                    name: data.name,
                    description: data.description,
                });
            })
            .catch(() => navigate("/dashboard"));
    }, [recipeId, navigate]);

    // -----------------------------
    // INGREDIENT GRID FETCH
    // -----------------------------

    const loadIngredients = useCallback(async () => {
        setIsLoading(true);

        const response = await fetch(
            `${API_BASE}/api/Ingredients/ingredients/${recipeId}/`,
            { credentials: "include" }
        );

        if (response.ok) {
            const data = await response.json();
            setIngredients(data.ingredients ?? data); // supports old + new API
            setValidUnits(data.validUnits ?? []);
        } else {
            console.error("Failed to fetch ingredients");
        }

        setIsLoading(false);
    }, [recipeId]);

    useEffect(() => {
        if (auth?.auth) loadIngredients();
    }, [auth, loadIngredients]);

    // -----------------------------
    // SAVE HANDLING
    // -----------------------------
    const handleSave = async (updated: Ingredient) => {
        setBanner("");

        const response = await fetch(`${API_BASE}/api/Ingredients/${updated.id}`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updated),
        });

        if (response.ok) {
            await loadIngredients();
            setBanner("Ingredient saved!");
        } else {
            setBanner("Error saving ingredient");
        }
    };


    // -----------------------------
    // DELETE HANDLING
    // -----------------------------

    const openDeleteModal = (ingredient: Ingredient | null, index: number | null) => {
        setIngredientToDelete(ingredient);
        setIngredientIndexToDelete(index);
        setModalIsOpen(!!ingredient);
    };

    const handleDelete = async () => {
        if (!ingredientToDelete) return;

        setBanner("");

        const response = await fetch(`${API_BASE}/api/Ingredients/${ingredientToDelete.id}`, {
            method: "DELETE",
            credentials: "include",
        });

        if (response.ok) {
            await loadIngredients();
            setBanner("Ingredient successfully deleted!");
        } else {
            setBanner("Error occurred during deletion");
        }

        setModalIsOpen(false);
    };

    // -----------------------------
    // SORT HANDLING
    // -----------------------------

    const handleDragEnd = async (event: any) => {
        setBanner("");

        const { active, over } = event;
        if (!over || active.id === over.id) return;

        const oldIndex = ingredients.findIndex((i) => i.id.toString() === active.id);
        const newIndex = ingredients.findIndex((i) => i.id.toString() === over.id);

        const reordered = [...ingredients];
        const [moved] = reordered.splice(oldIndex, 1);
        reordered.splice(newIndex, 0, moved);

        const updated = reordered.map((i, idx) => ({
            ...i,
            sortOrder: idx + 1,
        }));

        setIngredients(updated);

        const response = await fetch(
            `${API_BASE}/api/Ingredients/${recipeId}/update-sort-order`,
            {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(
                    updated.map((i) => ({
                        id: i.id,
                        sortOrder: i.sortOrder,
                    }))
                ),
            }
        );

        const result = await response.json();

        if (response.ok && result.success) {
            setBanner("Ingredients successfully re-ordered!");
        } else {
            setBanner("Error occurred during sorting");
        }
    };

    // -----------------------------
    // CONTROLLER OBJECT
    // -----------------------------

    const controller: IngredientGridController = {
        grid: {
            ingredients,
            validUnits,
        },
        modalIsOpen,
        ingredientToDelete,
        ingredientIndexToDelete,
        isLoading,
        handleDragEnd,
        handleDelete,
        handleSave,
        openDeleteModal,
    };

    // -----------------------------
    // RENDER
    // -----------------------------

    if (auth === null) return <Loader message="Loading ingredients ..." />;
    if (!auth.auth) return null;

    return (
        <>
            <div className="page-container w-100">
                <div
                    className={
                        isMobileTouchDevice()
                            ? "content-holder-mobile"
                            : "content-holder-desktop"
                    }
                >
                    <ProgressBar />

                    {isMobileTouchDevice() ? (
                        <IngredientsListMobile recipeId={recipeId} />
                    ) : (
                        <IngredientsListDesktop controller={controller} />
                    )}
                </div>
            </div>

            <ButtonGrid
                buttons={[
                    {
                        text: "Back",
                        url: `/recipes/recipeInfo/${recipeId}`,
                        icon: <Icon name="leftArrow" />,
                        type: "button",
                        mobileSlot: 1,
                        desktopSlot: 1,
                    },
                    {
                        text: "My Recipes",
                        url: "/recipes/myrecipes",
                        type: "button",
                        mobileSlot: 2,
                        desktopSlot: 3,
                    },
                    {
                        text: "Ingredient",
                        url: `/recipes/ingredients/add/${recipeId}`,
                        icon: <Icon name="add" />,
                        type: "button",
                        mobileSlot: 3,
                        desktopSlot: 5,
                    },
                ]}
            />
        </>
    );
}

export default Ingredients;
