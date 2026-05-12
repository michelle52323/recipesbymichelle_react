import { useEffect, useState } from "react";
import { getApiBaseUrl } from "../../../helpers/config";
import { TouchSensor } from '@dnd-kit/core';
import Modal from 'react-modal';
import { useOutletContext } from 'react-router-dom';
import { renderMathInHtml } from '../../../helpers/mathHelper'
import SortableIngredientItem from './SortableIngredientItem';
import Loader from '../../UserControls/Loader/Loader';

import {
    DndContext,
    closestCenter,
    useSensor,
    useSensors,
    PointerSensor,
    KeyboardSensor,
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    useSortable,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

Modal.setAppElement('#root'); // for accessibility

const API_BASE = getApiBaseUrl();

interface Ingredient {
    id: number;
    description: string;
    ingredientTypeId: number;
    ingredientType: {
        id: number;
        description: string;
        sortOrder: number;
        isActive: boolean;
    };
    sortOrder: number;
    isActive: boolean;
}

interface Props {
    recipeId: string;
}

function IngredientsListMobile({ recipeId }: Props) {
    const [ingredients, setIngredients] = useState<Ingredient[]>([]);

    const [modalIsOpen, setModalIsOpen] = useState(false);

    const [ingredientToDelete, setIngredientToDelete] = useState<Ingredient | null>(null);
    const [ingredientIndexToDelete, setIngredientIndexToDelete] = useState<number | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    const openDeleteModal = (ingredient: Ingredient, index: number) => {
        setIngredientToDelete(ingredient);
        setIngredientIndexToDelete(index);
        setModalIsOpen(true);
    };

    const { setBanner } = useOutletContext<{
        setBanner: (message: string) => void;
    }>();

    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(TouchSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    useEffect(() => {
        async function fetchIngredients() {
            setIsLoading(true);
            const response = await fetch(
                `${API_BASE}/api/Ingredients/${recipeId}/ingredients`,
                { credentials: "include" }
            );

            if (response.ok) {
                const data = await response.json();

                const transformed = data.map(i => ({
                    ...i,
                    description: renderMathInHtml(i.description)
                }));

                setIngredients(transformed);
                setIsLoading(false);
            } else {
                console.error("Failed to fetch ingredients");
                setIsLoading(false);
            }
        }

        fetchIngredients();
    }, [recipeId]);

    const handleDragEnd = async (event: any) => {
        setBanner('');
        const { active, over } = event;

        if (active.id !== over?.id) {
            const oldIndex = ingredients.findIndex(i => i.id.toString() === active.id);
            const newIndex = ingredients.findIndex(i => i.id.toString() === over?.id);

            const newOrder = arrayMove(ingredients, oldIndex, newIndex).map((ingredient, index) => ({
                ...ingredient,
                sortOrder: index + 1,
            }));

            setIngredients(newOrder);

            const response = await fetch(`${API_BASE}/api/Ingredients/${recipeId}/update-sort-order`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newOrder.map(i => ({
                    id: i.id,
                    sortOrder: i.sortOrder,
                }))),
            });

            const result = await response.json();

            if (response.ok && result.success) {
                setBanner('Ingredients successfully re-ordered!');
            } else {
                setBanner('Error occurred during sorting');
            }
        }
    };

    const handleDelete = async () => {
        setBanner('');

        const response = await fetch(`${API_BASE}/api/Ingredients/${ingredientToDelete?.id}`, {
            method: 'DELETE',
            credentials: 'include',
        });

        if (response.ok) {
            const refreshed = await fetch(`${API_BASE}/api/Ingredients/${recipeId}/ingredients`, {
                credentials: 'include',
            });

            if (refreshed.ok) {
                const data = await refreshed.json();
                setIngredients(data);
                setBanner('Recipe successfully deleted!');
            } else {
                setBanner('Recipe deleted, but failed to reload list.');
            }
        } else {
            setBanner('Error occurred during deletion');
        }

        setModalIsOpen(false);
    };

    if (isLoading) {
        return (
            <Loader message="Loading ingredients ..." />
        );
    }

    return (
        <div className="pt-3">

            <div className="content-inner-desktop">

                {ingredients.length === 0 && !isLoading ? (
                    <div className="empty-grid">No ingredients found. Start by creating one.</div>
                ) : (
                    <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
                        <SortableContext items={ingredients.map(i => i.id.toString())} strategy={verticalListSortingStrategy}>

                            {/* Header */}
                            <div className="d-flex align-items-start">

                                {/* Drag handle spacer */}
                                <div className="d-flex">
                                    <div className="drag-handle-width-desktop"></div>
                                </div>

                                {/* Main header columns */}
                                <div className="flex-grow-1">
                                    <div className="row">
                                        <div className="col-1 fw-bold">#</div>
                                        <div className="col-7 fw-bold">Description</div>
                                        <div className="col-4 fw-bold">Ingredient Type</div>
                                    </div>
                                </div>

                                {/* Right-side button placeholders */}
                                <div className="d-flex ms-3">
                                    <div className="fixed-button-icon"></div>
                                    <div className="fixed-button-icon"></div>
                                </div>
                            </div>

                            {/* Rows */}
                            <div className="grid-overflow-box gof-tall" id="sortable">
                                <div id="ingredients-list-container">
                                    {ingredients.map((ingredient, i) => (
                                        <SortableIngredientItem
                                            key={ingredient.id}
                                            ingredient={ingredient}
                                            index={i}
                                            openDeleteModal={() => openDeleteModal(ingredient, i + 1)}
                                            deviceType="mobile"
                                        />
                                    ))}
                                </div>

                            </div>

                        </SortableContext>
                    </DndContext>
                )}

            </div>

            <Modal
                isOpen={modalIsOpen}
                onRequestClose={() => setModalIsOpen(false)}
                contentLabel="Confirm Delete"
                className="dialog-wrapper"

            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">Confirm Delete</h5>
                    <button className="btn-close" onClick={() => setModalIsOpen(false)} ></button>
                </div>
                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">

                        {/* Fixed text stays normal */}
                        <div>
                            Are you sure you want to delete ingredient #{ingredientIndexToDelete})?
                        </div>

                        {/* Render the ingredient’s HTML safely */}
                        <div
                            className="mt-2"
                            dangerouslySetInnerHTML={{ __html: ingredientToDelete?.description ?? "" }}
                        />

                        {/* Hidden input stays separate */}
                        <input type="hidden" value={ingredientToDelete?.id} />
                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button
                            className="button button-modal"
                            onClick={() => {
                                setBanner(null);
                                setModalIsOpen(false);
                            }}
                        >
                            Cancel
                        </button>
                        <button className="button button-modal" onClick={handleDelete}>Yes, Delete</button>
                    </div>
                </div>

            </Modal>

        </div>


    );
}

export default IngredientsListMobile;
