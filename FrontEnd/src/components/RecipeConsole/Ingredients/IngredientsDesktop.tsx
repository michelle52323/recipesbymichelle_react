import React, { useState, useEffect } from "react";
import Modal from "react-modal";
import { DndContext, closestCenter } from "@dnd-kit/core";
import {
    SortableContext,
    verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import Loader from "../../UserControls/Loader/Loader";
import SortableIngredientItem from "./SortableIngredientItem";
import type { IngredientGridController } from "../../../types/Recipe/IngredientsGrid";



Modal.setAppElement("#root");

interface Props {
    controller: IngredientGridController;
}

function IngredientsListDesktop({ controller }: Props) {
    const {
        grid,
        modalIsOpen,
        handleDragEnd,
        handleDelete,
        handleSave,
        handleAdd,
        openDeleteModal,
        ingredientToDelete,
        ingredientIndexToDelete,
        isLoading
    } = controller;

    const ingredients = grid.ingredients;

    //const scrollRef = React.useRef<HTMLDivElement>(null);

    useEffect(() => {
            if (controller.scrollBoxRef.current) {
                controller.scrollBoxRef.current.scrollTop = controller.scrollBoxRef.current.scrollHeight;
            }
        }, [ingredients.length]);

    if (isLoading) {
        return <Loader message="Loading ingredients ..." />;
    }

    return (
        <div className="pt-3">
            <div className="content-inner-desktop">

                <DndContext
                    collisionDetection={closestCenter}
                    onDragEnd={handleDragEnd}
                >
                    <SortableContext
                        items={ingredients.map(i => i.id.toString())}
                        strategy={verticalListSortingStrategy}
                    >
                        {/* Header */}
                        <div className="d-flex align-items-start">
                            {/* LEFT: Drag + Qty + Unit headers */}
                            <div className="d-flex">
                                {/* Drag handle column */}
                                <div className="drag-handle-width-desktop"></div>

                                {/* Qty header (aligned with qty textbox) */}
                                <div className="fixed-textbox">
                                    <div className="fw-bold">Qty<span className="required" style={{ marginLeft: "-15px" }}></span></div>
                                </div>

                                {/* Unit header (aligned with unit textbox) */}
                                <div className="fixed-textbox">
                                    <div className="fw-bold">Unit</div>
                                </div>
                            </div>

                            {/* MIDDLE: Description + Instructions */}
                            <div className="flex-grow-1">
                                <div className="row">
                                    {/* Description (col-7) */}
                                    <div className="col-7 fw-bold">Description<span className="required" style={{ marginLeft: "-15px" }}></span></div>

                                    {/* Instructions (col-5) */}
                                    <div className="col-5 fw-bold">Instructions (optional)</div>

                                    {/* Placeholder for alignment (col-1) */}
                                    {/* <div className="col-1"></div> */}
                                </div>
                            </div>

                            {/* RIGHT: Save/Delete button columns */}
                            <div className="d-flex ms-3">
                                <div className="fixed-button-icon"></div>
                                <div className="fixed-button-icon"></div>
                            </div>
                        </div>


                        {/* Rows */}
                        <div className="grid-overflow-box gof-editable-short" id="sortable" ref={controller.scrollBoxRef}>
                            <div id="ingredients-list-container">
                                {ingredients.map((ingredient, i) => (
                                    <SortableIngredientItem
                                        key={ingredient.id}
                                        ingredient={ingredient}
                                        index={i}
                                        handleSave={handleSave}
                                        isAddRow={false}
                                        handleAdd={handleAdd}
                                        openDeleteModal={() =>
                                            openDeleteModal(ingredient, i + 1)
                                        }
                                        deviceType="desktop"
                                        recentlySavedId={controller.recentlySavedId}
                                        openValidationModal={controller.openValidationModal}
                                        pendingAction={controller.pendingAction}
                                        setPendingAction={controller.setPendingAction}
                                        unitLookupTable={controller.grid.unitLookupTable}
                                        measurementSystem={controller.measurementSystem}
                                    />
                                ))}
                            </div>
                            <div id="ingredients-list-container">

                                <SortableIngredientItem
                                    ingredient={controller.addRow}
                                    index={ingredients.length}
                                    isAddRow={true}
                                    handleAdd={handleAdd}
                                    deviceType="desktop"
                                    setAddRow={controller.setAddRow}
                                    openValidationModal={controller.openValidationModal}
                                    pendingAction={controller.pendingAction}
                                    setPendingAction={controller.setPendingAction}
                                    unitLookupTable={controller.grid.unitLookupTable}
                                    measurementSystem={controller.measurementSystem}
                                />

                            </div>
                        </div>
                    </SortableContext>
                </DndContext>
            </div>


        </div>
    );
}

export default IngredientsListDesktop;
