export interface RecipeBase {
    id: number;
    name: string | null;
    description: string | null;
    showAbbreviations: boolean;
    isActive: boolean;
    sortOrder: number;
    recipeVisibility: "MeOnly" | "AllUsers";

}

export interface RecipeView extends RecipeBase {
    // Added for React hydration
    ingredients?: Ingredient[];
    steps?: Step[];
    isMyRecipe?: boolean;
}


export interface Ingredient {
    id: number;
    quantity: number | null;
    unit: string | null;
    description: string | null;
    instructions: string | null;
    sortOrder: number | null;
    isActive: boolean;
}

export interface Step {
    id: number;
    description: string | null;
    sortOrder: number | null;
    isActive: boolean;
}
