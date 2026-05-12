import React, { useState } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import Icon from "../../UserControls/Icons/icons";
import type { Ingredient } from "../../../types/Recipe/Recipe";

interface Props {
    ingredient: Ingredient;
    index: number;
    handleSave: (updated: Ingredient) => void;
    openDeleteModal: (ingredient: Ingredient) => void;
    deviceType: "desktop";
}

const SortableIngredientItem: React.FC<Props> = ({
    ingredient,
    index,
    handleSave,
    openDeleteModal
}) => {

    // Local editable state
    const [qty, setQty] = useState(ingredient.quantity ?? "");
    const [unit, setUnit] = useState(ingredient.unit ?? "");
    const [desc, setDesc] = useState(ingredient.description ?? "");
    const [instr, setInstr] = useState(ingredient.instructions ?? "");

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({ id: ingredient.id.toString() });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    const onSave = () => {
        handleSave({
            ...ingredient,
            quantity: qty,
            unit: unit,
            description: desc,
            instructions: instr
        });
    };

    return (
        <div
            ref={setNodeRef}
            style={style}
            {...attributes}
            className="d-flex align-items-start grid-page-row grid-page-row-height-desktop sortable-container"
        >
            {/* LEFT: Drag + Qty + Unit */}
            <div className="d-flex">
                {/* Drag handle */}
                <div className="drag-handle-wrapper">
                    <div className="drag-handle drag-handle-width-desktop" {...listeners}>
                        <Icon name="drag" />
                    </div>
                </div>

                {/* Qty */}
                <div className="fixed-textbox">
                    <input
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={qty}
                        onChange={(e) => setQty(e.target.value)}
                    />
                </div>

                {/* Unit */}
                <div className="fixed-textbox">
                    <input
                        type="text"
                        className="form-control textbox textbox-small textbox-text"
                        maxLength={20}
                        value={unit}
                        onChange={(e) => setUnit(e.target.value)}
                    />
                </div>
            </div>

            {/* MIDDLE: Description + Instructions */}
            <div className="flex-grow-1">
                <div className="row">
                    {/* Hidden fields (match Razor structure) */}
                    <input type="hidden" value={ingredient.id} />
                    <input type="hidden" value={ingredient.sortOrder ?? index + 1} />

                    {/* Description */}
                    <div className="col-6">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={desc}
                            onChange={(e) => setDesc(e.target.value)}
                        />
                    </div>

                    {/* Instructions */}
                    <div className="col-6">
                        <input
                            type="text"
                            className="form-control textbox textbox-large textbox-text"
                            maxLength={50}
                            value={instr}
                            onChange={(e) => setInstr(e.target.value)}
                        />
                    </div>
                </div>
            </div>

            {/* RIGHT: Save + Delete buttons */}
            <div className="d-flex ps-3">

                {/* Save */}
                <div className="fixed-button-icon">
                    <button
                        className="button button-icon"
                        onClick={onSave}
                    >
                        <Icon name="save" />
                    </button>
                </div>

                {/* Delete */}
                <div className="fixed-button-icon">
                    <button
                        className="button button-icon button-icon-delete"
                        onClick={() => openDeleteModal(ingredient)}
                    >
                        <Icon name="delete" />
                    </button>
                </div>

            </div>
        </div>
    );
};

export default SortableIngredientItem;
