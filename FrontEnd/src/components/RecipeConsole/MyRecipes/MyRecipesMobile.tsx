import React, { useEffect, useState } from 'react';
import { useOutletContext, useNavigate } from 'react-router-dom';
import Modal from 'react-modal';
import { getApiBaseUrl } from '../../../helpers/config';
import '../../../grid-layout.css';
import SortableRecipeItem from './SortableRecipeItem';
const API_BASE = getApiBaseUrl();
import { TouchSensor } from '@dnd-kit/core';
import { isDevUseMockLogin, isMobileTouchDeviceDev, isMobileTouchDevice } from '../../../helpers/config';
import CategoryAssignmentModal from '../Categories/CategoryAssignmentModal';
import MobileRecipeActionsMenu from '../../UserControls/SubMenus/MyRecipes/MobileRecipeActionsMenu';
import Loader from '../../UserControls/Loader/Loader';
import type { Category } from '../../../types/Categories/Categories';

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
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

Modal.setAppElement('#root'); // for accessibility

interface Recipe {
    id: number;
    name: string;
    description: string;
    sortOrder: number;
    //categorySortOrder: number;
    categories?: Category[];
}

interface MyRecipesMobileProps {
    showCategories: boolean;
    showCategoryToolbar: boolean;
    openCategory: Category;
    setOpenCategory: (value: Category) => void;
    currentView: "Recipes" | "Categories" | null;
    setCurrentView: (value: "Recipes" | "Categories" | null) => void;
}


const MyRecipesMobile: React.FC<MyRecipesMobileProps> = ({
    showCategories,
    showCategoryToolbar,
    openCategory,
    setOpenCategory,
    currentView,
    setCurrentView
}) => {

    const navigate = useNavigate();

    const [recipes, setRecipes] = useState<Recipe[]>([]);

    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [recipeToDelete, setRecipeToDelete] = useState<{ id: number; name: string } | null>(null);

    const [selectedRecipe, setSelectedRecipe] = useState<Recipe | null>(null);
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isClosing, setIsClosing] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    const [assignCategoriesModalIsOpen, setAssignCategoriesModalIsOpen] = useState(false);
    const [allCategories, setAllCategories] = useState<Category[]>([]);
    const [recipeCategories, setRecipeCategories] = useState<Category[]>([]);
    const [recipeToAssign, setRecipeToAssign] = useState<{ id: number; name: string; categories?: Category[] } | null>(null);

    const openDeleteModal = (recipe: { id: number; name: string }) => {
        setRecipeToDelete(recipe);
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
        const fetchRecipes = async () => {
            setIsLoading(true);
            const mode = import.meta.env.VITE_MODE;
            const url = isDevUseMockLogin() && mode == "dev"
                ? `${API_BASE}/api/MyRecipes/getRecipesMock`
                : `${API_BASE}/api/MyRecipes/getRecipes`;

            const response = await fetch(url, {
                credentials: 'include',
            });

            if (!response.ok) {
                console.error('Failed to fetch recipes');
                setIsLoading(false);
                return;
            }

            const data = await response.json();

            // // ⭐ Apply filtering logic
            // const filtered = !showCategories
            //     ? data
            //     : data.filter(r => r.categoryId === openCategory.id);
            const filtered = !showCategories || openCategory == null
                ? data
                : data
                    // Filter recipes that belong to the open category
                    .filter(r =>
                        r.categories?.some(c => c.id === openCategory.id)
                    )
                    // Sort by the sortOrder inside that category
                    .sort((a, b) => {
                        const aCat = a.categories?.find(c => c.id === openCategory.id);
                        const bCat = b.categories?.find(c => c.id === openCategory.id);

                        const aSort = aCat?.sortOrder ?? 0;
                        const bSort = bCat?.sortOrder ?? 0;

                        return aSort - bSort;
                    });

            setRecipes(filtered);
            setIsLoading(false);
        };

        fetchRecipes();
    }, [showCategories, openCategory]);


    const handleDragEnd = async (event: any) => {
        setBanner('');
        if (openCategory == null) {
            const { active, over } = event;
            if (active.id !== over?.id) {
                const oldIndex = recipes.findIndex(r => r.id.toString() === active.id);
                const newIndex = recipes.findIndex(r => r.id.toString() === over?.id);
                const newOrder = arrayMove(recipes, oldIndex, newIndex).map((recipe, index) => ({
                    ...recipe,
                    sortOrder: index + 1,
                }));

                setRecipes(newOrder);

                const response = await fetch(API_BASE + `/api/MyRecipes/updateSortOrder`, {
                    method: 'POST',
                    credentials: 'include',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(newOrder.map(r => ({
                        id: r.id,
                        sortOrder: r.sortOrder,
                    }))),
                });

                const result = await response.json();

                if (response.ok && result.success) {
                    setBanner('Recipes successfully re-ordered!');
                } else {
                    setBanner('Error occurred during sorting');
                }
            }
        }
        else {
            const { active, over } = event;
            if (active.id !== over?.id) {
                const oldIndex = recipes.findIndex(r => r.id.toString() === active.id);
                const newIndex = recipes.findIndex(r => r.id.toString() === over?.id);

                const newOrder = arrayMove(recipes, oldIndex, newIndex).map((recipe, index) => ({
                    ...recipe,
                    sortOrder: index + 1,
                }));

                setRecipes(newOrder);

                const response = await fetch(API_BASE + `/api/RecipesCategories/updateSortOrder`, {
                    method: 'POST',
                    credentials: 'include',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(newOrder.map(r => ({
                        recipeId: r.id,
                        categoryId: openCategory.id,
                        sortOrder: r.sortOrder,
                    }))),
                });

                const result = await response.json();

                if (response.ok && result.success) {
                    setBanner('Recipes successfully re-ordered!');
                } else {
                    setBanner('Error occurred during recipe sorting');
                }
            }
        }

    };

    const handleDelete = async () => {
        setBanner('');

        const response = await fetch(`${API_BASE}/api/MyRecipes/${recipeToDelete?.id}`, {
            method: 'DELETE',
            credentials: 'include',
        });

        if (response.ok) {
            const refreshed = await fetch(`${API_BASE}/api/MyRecipes/getRecipes`, {
                credentials: 'include',
            });

            if (refreshed.ok) {
                const data = await refreshed.json();
                const filtered = !showCategories || openCategory == null
                    ? data
                    : data
                        // Filter recipes that belong to the open category
                        .filter(r =>
                            r.categories?.some(c => c.id === openCategory.id)
                        )
                        // Sort by the sortOrder inside that category
                        .sort((a, b) => {
                            const aCat = a.categories?.find(c => c.id === openCategory.id);
                            const bCat = b.categories?.find(c => c.id === openCategory.id);

                            const aSort = aCat?.sortOrder ?? 0;
                            const bSort = bCat?.sortOrder ?? 0;

                            return aSort - bSort;
                        });

                setRecipes(filtered);
                setBanner('Recipe successfully deleted!');
            } else {
                setBanner('Recipe deleted, but failed to reload list.');
            }
        } else {
            setBanner('Error occurred during deletion');
        }

        setModalIsOpen(false);
    };

    // BEGIN Category Assignment
    useEffect(() => {


        getCategories("Alphabetical");
    }, []);

    const getCategories = async (sortBy: "Alphabetical" | "SortOrder" | null) => {
        const endpoint = `${API_BASE}/api/Categories/list${isDevUseMockLogin() ? "mock" : ""}?sortBy=${sortBy}`;
        const response = await fetch(endpoint, {
            method: "GET",
            credentials: "include", // ← always include credentials
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            //setCategoriesIsLoading(false);
            throw new Error("Failed to fetch categories");

        }
        else {
            const data = await response.json();   // ← THIS is the important part
            setAllCategories(data);                // ← store the actual category array
            //setCategoriesIsLoading(false);

        }

    };

    const handleOpenAssignCategories = (recipe: { id: number; name: string; categories?: Category[] }) => {
        setRecipeToAssign(recipe);
        setRecipeCategories(recipe.categories);
        setAssignCategoriesModalIsOpen(true);
    };

    const handleCloseAssignCategories = () => {
        setAssignCategoriesModalIsOpen(false);
    };

    const handleSaveAssignedCategories = async (selectedCategoryIds: number[]) => {
        setBanner('');
        try {
            // Build DTO payload from selected categories
            const updated = allCategories
                .filter(c => selectedCategoryIds.includes(c.id))
                .map(c => ({
                    id: c.id,
                    name: c.name,       // backend ignores this, but DTO requires it
                    sortOrder: c.sortOrder ?? 0
                }));

            // console.log("RecipeId: " + recipeToAssign.id);
            // console.log("DATA:" + JSON.stringify(updated));


            // Call backend API
            const response = await fetch(
                `${API_BASE}/api/RecipesCategories/update-recipe-categories/${recipeToAssign.id}`,
                {
                    method: "PUT",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(updated)
                }
            );

            if (!response.ok) {
                setRecipeCategories(
                    allCategories.filter(c => selectedCategoryIds.includes(c.id))
                );

                throw new Error("Failed to update recipe categories");
            }
            else {
                // Update local UI state
                setRecipeCategories(null);

                // ⭐ Update the main recipes list so the UI reflects the new categories
                setRecipes(prev =>
                    prev.map(r =>
                        r.id === recipeToAssign.id
                            ? { ...r, categories: updated }
                            : r
                    )
                );

                // Remove recipe from display if it no longer belongs to the open category
                if (showCategories && openCategory) {
                    const stillInCategory = updated.some(c => c.id === openCategory.id);

                    if (!stillInCategory) {
                        setRecipes(prev => prev.filter(r => r.id !== recipeToAssign.id));
                    }
                }

                setBanner('Category assignment successful!');


                setAssignCategoriesModalIsOpen(false);
            }



        } catch (err) {
            console.error("Error saving categories", err);
        }
    };

    // END Category Assignment

    const closeMenu = () => {
        setIsClosing(true);

        setTimeout(() => {
            setIsMenuOpen(false);
            setSelectedRecipe(null);
            setIsClosing(false);
        }, 150);
    };

    if (isLoading) {
        return (
            <Loader message="Loading recipes ..." />
        );
    }

    const gofClassName = showCategoryToolbar ? "gof-editable-mobile-short" : "gof-editable-mobile";

    return (
        <div className="page-container w-100 pt-3">

            <div className="content-inner-desktop">

                {recipes.length === 0 && !isLoading ? (
                    <div className="empty-grid">No recipes found. Start by creating one.</div>
                ) : (
                    <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
                        <SortableContext items={recipes.map(r => r.id.toString())} strategy={verticalListSortingStrategy}>
                            {/* Header */}
                            <div className="d-flex align-items-start">
                                <div className="d-flex">
                                    <div className="drag-handle-width-desktop"></div>
                                </div>

                                <div className="flex-grow-1">
                                    <div className="row">
                                        {/* <div className="col-6 col-custom-6-12 fw-bold">Name</div>
                                        <div className="col-6 col-custom-6-0 fw-bold">Description</div> */}
                                        &nbsp;
                                    </div>
                                </div>

                                <div className="d-flex ms-3">

                                </div>
                            </div>

                            {/* Rows */}
                            <div className={`grid-overflow-box ${gofClassName}`} id="sortable">
                                {recipes.map((recipe, i) => (
                                    <SortableRecipeItem
                                        key={recipe.id}
                                        recipe={recipe}
                                        index={i}
                                        isMobile={true}
                                        openDeleteModal={() => openDeleteModal(recipe)}
                                        setSelectedRecipe={setSelectedRecipe}
                                        setIsMenuOpen={setIsMenuOpen}
                                    />
                                ))}
                            </div>
                        </SortableContext>
                    </DndContext>
                )}

            </div>

            {assignCategoriesModalIsOpen && (
                <CategoryAssignmentModal
                    show={assignCategoriesModalIsOpen}
                    onClose={handleCloseAssignCategories}
                    onSave={handleSaveAssignedCategories}
                    allCategories={allCategories}
                    recipeCategories={recipeCategories}
                    recipe={recipeToAssign}
                />

            )}

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
                        Are you sure you want to delete recipe "{recipeToDelete?.name}"?
                        <input type="hidden" value={recipeToDelete?.id} />
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

            {isMenuOpen && selectedRecipe && (
                <>
                    <div
                        className="mobile-menu-backdrop"
                        onClick={closeMenu}
                        style={{
                            position: 'fixed',
                            inset: 0,
                            background: 'rgba(0,0,0,0.4)',
                            zIndex: 9998
                        }}
                    ></div>

                    <MobileRecipeActionsMenu
                        recipe={selectedRecipe}
                        navigate={(path: string) => navigate(path)}
                        openDeleteModal={() => openDeleteModal(selectedRecipe)}
                        closeMenu={closeMenu}
                        isClosing={isClosing}
                        handleOpenAssignCategories={() => handleOpenAssignCategories(selectedRecipe)}

                    />
                </>
            )}

        </div>

    );
};

export default MyRecipesMobile;
