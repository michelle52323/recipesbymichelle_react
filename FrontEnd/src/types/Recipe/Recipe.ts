import type { Category } from "../Categories/Categories";
export interface RecipeBase {
    id: number;
    name: string | null;
    description: string | null;
    showAbbreviations: boolean;
    isActive: boolean;
    sortOrder: number;
    recipeVisibility: "MeOnly" | "AllUsers";
    recipeFont: "SansSerif" | "Serif" | "Handwritten";
    categories?: Category[];

}

export interface RecipeMyRecipes extends RecipeBase{
    
}

export interface RecipeView extends RecipeBase {
    // Added for React hydration
    ingredients?: Ingredient[];
    steps?: Step[];
    isMyRecipe?: boolean;
    measurementSystem?: "Imperial" | "Metric" | null;
}


export interface Ingredient {
    id: number;
    quantity: string | null;
    quantityMax: string | null;
    unit: string | null;
    description: string | null;
    instructions: string | null;
    sortOrder: number | null;
    isActive: boolean;
}

export interface IngredientAdd extends Ingredient{
    recipeId: number;
}

export interface Step {
    id: number;
    description: string | null;
    sortOrder: number | null;
    isActive: boolean;
}

export interface StepAdd extends Step{
    recipeId: number;
}

export interface Unit {
    id: number;
    description: string;
    abbreviation: string;
    plural: string | null;
    system: number | null;
}

export interface FavoriteBase {
    id: number;
    userId: number;
    recipeId: number;
    sortOrder: number;
    isMine: boolean;
    isFavorite: boolean;
}


export interface Favorite extends FavoriteBase {
    recipe: RecipeBase;
}


export enum RecipeFont {
    SansSerif = 1,
    Serif = 2,
    Handwritten = 3
}

