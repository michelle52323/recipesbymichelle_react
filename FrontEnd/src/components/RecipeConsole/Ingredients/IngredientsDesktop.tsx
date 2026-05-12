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
        openDeleteModal,
        ingredientToDelete,
        ingredientIndexToDelete,
        isLoading
    } = controller;

    const ingredients = grid.ingredients;

    if (isLoading) {
        return <Loader message="Loading ingredients ..." />;
    }

    return (
        <div className="pt-3">
            <div className="content-inner-desktop">

                {ingredients.length === 0 ? (
                    <div className="empty-grid">
                        No ingredients found. Start by creating one.
                    </div>
                ) : (
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
                                <div className="d-flex">
                                    <div className="drag-handle-width-desktop"></div>
                                </div>

                                <div className="flex-grow-1">
                                    <div className="row">
                                        <div className="col-1 fw-bold">#</div>
                                        <div className="col-7 fw-bold">Description</div>
                                        <div className="col-4 fw-bold">Ingredient Type</div>
                                    </div>
                                </div>

                                <div className="d-flex ms-3">
                                    <div className="fixed-button-icon"></div>
                                    <div className="fixed-button-icon"></div>
                                </div>
                            </div>

                            {/* Rows */}
                            <div className="grid-overflow-box gof-editable" id="sortable">
                                <div id="ingredients-list-container">
                                    {ingredients.map((ingredient, i) => (
                                        <SortableIngredientItem
                                            key={ingredient.id}
                                            ingredient={ingredient}
                                            index={i}
                                            handleSave={handleSave}
                                            openDeleteModal={() =>
                                                openDeleteModal(ingredient, i + 1)
                                            }
                                            deviceType="desktop"
                                        />
                                    ))}
                                </div>
                            </div>
                        </SortableContext>
                    </DndContext>
                )}
            </div>

            {/* DELETE MODAL */}
            <Modal
                isOpen={modalIsOpen}
                onRequestClose={() => openDeleteModal(null, null)}
                contentLabel="Confirm Delete"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">Confirm Delete</h5>
                    <button
                        className="btn-close"
                        onClick={() => openDeleteModal(null, null)}
                    ></button>
                </div>

                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        <div>
                            Are you sure you want to delete ingredient #
                            {ingredientIndexToDelete})?
                        </div>

                        <div
                            className="mt-2"
                            dangerouslySetInnerHTML={{
                                __html: ingredientToDelete?.description ?? "",
                            }}
                        />

                        <input type="hidden" value={ingredientToDelete?.id} />
                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button
                            className="button button-modal"
                            onClick={() => openDeleteModal(null, null)}
                        >
                            Cancel
                        </button>

                        <button
                            className="button button-modal"
                            onClick={handleDelete}
                        >
                            Yes, Delete
                        </button>
                    </div>
                </div>
            </Modal>
        </div>
    );
}

export default IngredientsListDesktop;
