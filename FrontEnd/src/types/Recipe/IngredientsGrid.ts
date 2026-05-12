import type { DragEndEvent } from "@dnd-kit/core";
import type {Ingredient, Unit} from './Recipe';

/* Ingredient grid types */
export interface IngredientGridController {
    grid: IngredientGrid;
    modalIsOpen: boolean;

    ingredientToDelete: Ingredient | null;
    ingredientIndexToDelete: number | null;
    isLoading: boolean;

    handleDragEnd: (event: DragEndEvent) => void;
    handleDelete: () => void;
    handleSave: (updated: Ingredient) => void; 
    openDeleteModal: (ingredient: Ingredient | null, index: number | null) => void;
}


export interface IngredientGrid {
    ingredients: Ingredient[];
    validUnits: Unit[];
}
