import type { DragEndEvent } from "@dnd-kit/core";
import type { Step } from './Recipe';
import type { MeasurementUnit } from "../Measurement/MeasurementType";

/* Step grid types */
export interface StepGridController {
    grid: StepGrid;
    modalIsOpen: boolean;

    stepToDelete: Step | null;
    stepIndexToDelete: number | null;
    isLoading: boolean;

    handleDragEnd: (event: DragEndEvent) => void;
    handleDelete: () => void;
    handleSave: (updated: Step) => void;
    handleAdd: (added: Step) => void;
    openDeleteModal: (step: Step | null, index: number | null) => void;
    recentlySavedId: number | null;
    addRow: Step;
    setAddRow: React.Dispatch<React.SetStateAction<Step>>;

    measurementSystem: "Imperial" | "Metric" | null;

    //DELETE MODAL
    validationModalIsOpen: boolean;
    validationErrors: string[];
    openValidationModal: (errors: string[]) => void;
    closeValidationModal: () => void;

    // PENDING ACTION + STEP
    pendingAction: "add" | "save" | null;

    setPendingAction: React.Dispatch<React.SetStateAction<"add" | "save" | null>>;

    //MOBILE open state
    openId: string | null;
    setOpenId: React.Dispatch<React.SetStateAction<string | null>>;
    onToggle: (id: string) => void;
    ADD_ID: "ADD_ROW";

    //MOBILE scrolling
    scrollBoxRef: React.RefObject<HTMLDivElement>

}


export interface StepGrid {
    steps: Step[];
    unitLookupTable: MeasurementUnit[];
}
