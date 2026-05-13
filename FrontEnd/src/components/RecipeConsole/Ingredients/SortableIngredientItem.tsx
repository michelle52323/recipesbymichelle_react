import React, { useState, useEffect } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import Icon from "../../UserControls/Icons/icons";
import type { Ingredient } from "../../../types/Recipe/Recipe";

interface Props {
    ingredient: Ingredient | null;
    index: number;

    // Normal row
    handleSave?: (ingredient: Ingredient) => void;
    openDeleteModal?: () => void;

    // Add row
    isAddRow?: boolean;
    handleAdd?: (added: Ingredient) => void;

    deviceType: "desktop" | "mobile";

    recentlySavedId: number | null;
    setAddRow?: React.Dispatch<React.SetStateAction<Ingredient>>;

    // NEW: validation + pending action
    openValidationModal: (
        errors: string[],
        action: "add" | "save",
        ingredient: Ingredient
    ) => void;

    pendingAction: "add" | "save" | null;
    setPendingAction: React.Dispatch<React.SetStateAction<"add" | "save" | null>>;

}


const SortableIngredientItem: React.FC<Props> = ({
    ingredient,
    index,
    handleSave,
    handleAdd,
    openDeleteModal,
    isAddRow = false,
    deviceType,
    recentlySavedId,
    setAddRow,
    openValidationModal,
    pendingAction,
    setPendingAction
}) => {

    // Local editable state
    // For normal rows, keep local state.
    // For Add Row, derive values from parent-controlled ingredient.
    const [localQty, setLocalQty] = useState(ingredient?.quantity ?? "");
    const [localUnit, setLocalUnit] = useState(ingredient?.unit ?? "");
    const [localDesc, setLocalDesc] = useState(ingredient?.description ?? "");
    const [localInstr, setLocalInstr] = useState(ingredient?.instructions ?? "");

    // Unified values used by the UI
    const qty = isAddRow ? ingredient?.quantity ?? "" : localQty;
    const unit = isAddRow ? ingredient?.unit ?? "" : localUnit;
    const desc = isAddRow ? ingredient?.description ?? "" : localDesc;
    const instr = isAddRow ? ingredient?.instructions ?? "" : localInstr;


    const qtyRef = React.useRef<HTMLInputElement>(null);
    const isHighlighted = !isAddRow && ingredient?.id === recentlySavedId;


    // const { attributes, listeners, setNodeRef, transform, transition } =
    //     useSortable({ id: ingredient.id.toString() });
    const sortable = !isAddRow
        ? useSortable({ id: ingredient!.id.toString() })
        : {
            attributes: {},
            listeners: {},
            setNodeRef: () => { },
            transform: null,
            transition: null
        };

    const { attributes, listeners, setNodeRef, transform, transition } = sortable;


    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    const validate = () => {
        const errors: string[] = [];

        if (!qty.trim()) errors.push("Quantity is required.");
        if (!desc.trim()) errors.push("Description is required.");

        // Optional: add unit/qty format validation later

        return errors;
    };


    // const onSave = () => {
    //     handleSave({
    //         ...ingredient,
    //         quantity: qty,
    //         unit: unit,
    //         description: desc,
    //         instructions: instr
    //     });
    // };

    const onSave = () => {
        const errors = validate();

        if (errors.length > 0) {
            setPendingAction("save");

            openValidationModal(
                errors,
                "save",
                {
                    ...ingredient!,
                    quantity: qty,
                    unit: unit,
                    description: desc,
                    instructions: instr
                }
            );

            return;
        }

        // No validation errors → perform save
        handleSave?.({
            ...ingredient!,
            quantity: qty,
            unit: unit,
            description: desc,
            instructions: instr
        });
    };


    // const onAdd = () => {
    //     handleAdd?.({
    //         id: 0,
    //         quantity: qty,
    //         unit: unit,
    //         description: desc,
    //         instructions: instr,
    //         sortOrder: index + 1,
    //         isActive: true
    //     });
    // };

    // useEffect(() => {
    //     if (isAddRow) {
    //         qtyRef.current?.focus();
    //     }
    // }, [isAddRow]);

    const onAdd = () => {
        const errors = validate();

        if (errors.length > 0) {
            setPendingAction("add");

            openValidationModal(
                errors,
                "add",
                {
                    id: 0,
                    quantity: qty,
                    unit: unit,
                    description: desc,
                    instructions: instr,
                    sortOrder: index + 1,
                    isActive: true
                }
            );

            return;
        }

        // No validation errors → perform add
        handleAdd?.({
            id: 0,
            quantity: qty,
            unit: unit,
            description: desc,
            instructions: instr,
            sortOrder: index + 1,
            isActive: true
        });
    };


    useEffect(() => {
        if (isAddRow) {
            const isBlank =
                (ingredient?.quantity ?? "") === "" &&
                (ingredient?.unit ?? "") === "" &&
                (ingredient?.description ?? "") === "" &&
                (ingredient?.instructions ?? "") === "";

            if (isBlank) {
                qtyRef.current?.focus();
            }
        }
    }, [
        isAddRow,
        ingredient?.quantity,
        ingredient?.unit,
        ingredient?.description,
        ingredient?.instructions
    ]);


    useEffect(() => {

        if (!isAddRow && ingredient?.id === recentlySavedId) {
            qtyRef.current?.focus();
        }
    }, [recentlySavedId, isAddRow, ingredient]);




    return (
        <div
            ref={setNodeRef}
            style={style}
            {...(!isAddRow ? attributes : {})}
            className="d-flex align-items-start grid-page-row grid-page-row-height-desktop sortable-container"
        >


            {/* LEFT: Drag + Qty + Unit */}
            <div className="d-flex">
                {/* Drag handle */}
                <div className="drag-handle-wrapper">
                    {!isAddRow ? (
                        <div className="drag-handle drag-handle-width-desktop" {...listeners}>
                            <Icon name="drag" />
                        </div>
                    ) : (
                        <div className="drag-handle-width-desktop">&nbsp;</div>
                    )}
                </div>

                {/* Qty */}
                <div className="fixed-textbox">
                    <input
                        ref={qtyRef}
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={qty}
                        onChange={(e) => {
                            if (isAddRow) {
                                setAddRow?.(prev => ({ ...prev, quantity: e.target.value }));
                            } else {
                                setLocalQty(e.target.value);
                            }
                        }}

                    />

                </div>

                {/* Unit */}
                <div className="fixed-textbox">
                    <input
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={unit}
                        onChange={(e) => {
                            if (isAddRow) {
                                setAddRow?.(prev => ({ ...prev, unit: e.target.value }));
                            } else {
                                setLocalUnit(e.target.value);
                            }
                        }}

                    />
                </div>
            </div>

            {/* MIDDLE: Description + Instructions */}
            <div className="flex-grow-1">
                <div className="row">

                    {!isAddRow && (
                        <>
                            <input type="hidden" value={ingredient!.id} />
                            <input type="hidden" value={ingredient!.sortOrder ?? index + 1} />
                        </>
                    )}

                    {/* Description (col-7) */}
                    <div className="col-7">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={desc}
                            onChange={(e) => {
                                if (isAddRow) {
                                    setAddRow?.(prev => ({ ...prev, description: e.target.value }));
                                } else {
                                    setLocalDesc(e.target.value);
                                }
                            }}

                        />
                    </div>

                    {/* Instructions (col-5) */}
                    <div className="col-5">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={instr}
                            onChange={(e) => {
                                if (isAddRow) {
                                    setAddRow?.(prev => ({ ...prev, instructions: e.target.value }));
                                } else {
                                    setLocalInstr(e.target.value);
                                }
                            }}

                        />
                    </div>

                    {/* Qty column placeholder (col-1) */}
                    {/* <div className="col-1">&nbsp;</div> */}
                </div>
            </div>

            {/* RIGHT: Save + Delete */}
            <div className="d-flex ps-3">

                {/* Save / Add */}
                <div className="fixed-button-icon">
                    {!isAddRow ? (
                        <button className="button button-icon" onClick={onSave}>
                            <Icon name="save" />
                        </button>
                    ) : (
                        <button className="button button-icon" onClick={onAdd}>
                            <Icon name="add" />
                        </button>

                    )}
                </div>

                {/* Delete */}
                <div className="fixed-button-icon">
                    {!isAddRow ? (
                        <button
                            className="button button-icon button-icon-delete"
                            onClick={() => openDeleteModal?.(ingredient!)}
                        >
                            <Icon name="delete" />
                        </button>
                    ) : (
                        <span className="button-icon">&nbsp;</span>
                    )}
                </div>

            </div>
        </div>

    );
};

export default SortableIngredientItem;
