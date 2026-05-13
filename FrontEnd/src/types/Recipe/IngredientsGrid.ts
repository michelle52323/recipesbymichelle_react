import type { DragEndEvent } from "@dnd-kit/core";
import type { Ingredient } from './Recipe';
import type { MeasurementUnit } from "../Measurement/MeasurementType";

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
    handleAdd: (added: Ingredient) => void;
    openDeleteModal: (ingredient: Ingredient | null, index: number | null) => void;
    recentlySavedId: number | null;
    addRow: Ingredient;
    setAddRow: React.Dispatch<React.SetStateAction<Ingredient>>;

    measurementSystem: "Imperial" | "Metric" | null;

    //DELETE MODAL
    validationModalIsOpen: boolean;
    validationErrors: string[];
    openValidationModal: (errors: string[]) => void;
    closeValidationModal: () => void;

    // PENDING ACTION + INGREDIENT
    pendingAction: "add" | "save" | null;

    setPendingAction: React.Dispatch<React.SetStateAction<"add" | "save" | null>>;
}


export interface IngredientGrid {
    ingredients: Ingredient[];
    unitLookupTable: MeasurementUnit[];
}
