import React, { useEffect, useState } from 'react';
import { useOutletContext, useNavigate } from 'react-router-dom';
import Modal from 'react-modal';
import { getApiBaseUrl } from '../../../helpers/config';
import '../../../grid-layout.css';
import SortableCategoryItem from './SortableCategoryItem';
const API_BASE = getApiBaseUrl();
import { TouchSensor } from '@dnd-kit/core';
import { isDevUseMockLogin } from '../../../helpers/config';
import Loader from '../../UserControls/Loader/Loader';
import CategoriesActionsMenu from '../../UserControls/SubMenus/Categories/CategoriesActionsMenu';
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

Modal.setAppElement('#root');



interface CategoryListProps {
    categories: Category[];
    setCategories: (value: Category[]) => void;
    showCategories: boolean;
    setShowCategories: (value: boolean) => void;
    categorySortBy: number;
    setCategorySortBy: (value: number) => void;
    openCategory: Category;
    setOpenCategory: (value: Category) => void;
    currentView: "Recipes" | "Categories" | null;
    setCurrentView: (value: "Recipes" | "Categories" | null) => void;
}


const CategoryList: React.FC<CategoryListProps> = ({
    categories,
    setCategories,
    showCategories,
    setShowCategories,
    categorySortBy,
    setCategorySortBy,
    openCategory,
    setOpenCategory,
    currentView,
    setCurrentView }) => {

    const navigate = useNavigate();

    //const [categories, setCategories] = useState<Category[]>([]);
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [categoryToDelete, setCategoryToDelete] = useState<{ id: number; name: string } | null>(null);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isClosing, setIsClosing] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    const { setBanner } = useOutletContext<{ setBanner: (message: string) => void }>();

    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(TouchSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    // useEffect(() => {
    //     setIsLoading(true);

    //     const mode = import.meta.env.VITE_MODE;
    //     const url = isDevUseMockLogin() && mode === "dev"
    //         ? `${API_BASE}/api/Categories/getCategoriesMock`
    //         : `${API_BASE}/api/Categories/getCategories`;

    //     const fetchCategories = async () => {
    //         const response = await fetch(url, { credentials: 'include' });
    //         if (response.ok) {
    //             const data = await response.json();
    //             setCategories(data);
    //             setIsLoading(false);
    //         } else {
    //             console.error('Failed to fetch categories');
    //             setIsLoading(false);
    //         }
    //     };

    //     fetchCategories();
    // }, []);

    // useEffect(() => {
    //     if (!categories || categories.length === 0) return;

    //     let sorted = [...categories];

    //     if (categorySortBy === 2) {
    //         sorted.sort((a, b) => a.sortOrder - b.sortOrder);
    //     } else if (categorySortBy === 1) {
    //         sorted.sort((a, b) => a.name.localeCompare(b.name));
    //     }

    //     setCategories(sorted);
    // }, [categorySortBy]);



    const handleDragEnd = async (event: any) => {
        setBanner('');
        const { active, over } = event;

        if (active.id !== over?.id) {
            const oldIndex = categories.findIndex(c => c.id.toString() === active.id);
            const newIndex = categories.findIndex(c => c.id.toString() === over?.id);

            const newOrder = arrayMove(categories, oldIndex, newIndex).map((category, index) => ({
                ...category,
                sortOrder: index + 1,
            }));

            setCategories(newOrder);

            const response = await fetch(API_BASE + `/api/Categories/updateSortOrder`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newOrder.map(c => ({
                    id: c.id,
                    sortOrder: c.sortOrder,
                }))),
            });

            const result = await response.json();

            if (response.ok && result.success) {
                setBanner('Categories successfully re-ordered!');
            } else {
                setBanner('Error occurred during sorting');
            }
        }
    };

    const openDeleteModal = (recipe: { id: number; name: string }) => {
        //setRecipeToDelete(recipe);
        //setModalIsOpen(true);
    };

    const handleDelete = async () => {
        setBanner('');

        const response = await fetch(`${API_BASE}/api/Categories/${categoryToDelete?.id}`, {
            method: 'DELETE',
            credentials: 'include',
        });

        if (response.ok) {
            const refreshed = await fetch(`${API_BASE}/api/Categories/getCategories`, {
                credentials: 'include',
            });

            if (refreshed.ok) {
                const data = await refreshed.json();
                setCategories(data);
                setBanner('Category successfully deleted!');
            } else {
                setBanner('Category deleted, but failed to reload list.');
            }
        } else {
            setBanner('Error occurred during deletion');
        }

        setModalIsOpen(false);
    };

    const closeMenu = () => {
        setIsClosing(true);

        setTimeout(() => {
            setIsMenuOpen(false);
            setSelectedCategory(null);
            setIsClosing(false);
        }, 150);
    };

    if (isLoading) {
        return <Loader message="Loading categories ..." />;
    }

    //const gofClassName = showCategoryToolbar ? "gof-editable-mobile-short" : "gof-editable-mobile";
    const gofClassName = "gof-editable-mobile-short"

    return (
        <div className="page-container w-100 pt-3">
            <div className="content-inner-desktop">

                {categories.length === 0 && !isLoading ? (
                    <div className="empty-grid">No categories found. Start by creating one.</div>
                ) : (
                    <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
                        <SortableContext items={categories.map(c => c.id.toString())} strategy={verticalListSortingStrategy}>

                            <div className="d-flex align-items-start ">
                                <div className="d-flex">
                                    <div className="drag-handle-width-desktop"></div>
                                </div>

                                <div className="flex-grow-1">
                                    <div className="row">&nbsp;</div>
                                </div>

                                <div className="d-flex ms-3"></div>
                            </div>

                            <div className={`grid-overflow-box ${gofClassName}`} id="sortable">

                                {categories.map((category, i) => (
                                    <SortableCategoryItem
                                        key={category.id}
                                        category={category}
                                        index={i}
                                        isMobile={true}
                                        openDeleteModal={() => setCategoryToDelete(category)}
                                        setSelectedCategory={setSelectedCategory}
                                        setIsMenuOpen={setIsMenuOpen}
                                        openCategory={openCategory}
                                        setOpenCategory={setOpenCategory}
                                        currentView={currentView}
                                        setCurrentView={setCurrentView}
                                    />
                                ))}
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
                        Are you sure you want to delete category "{categoryToDelete?.name}"?
                        <input type="hidden" value={categoryToDelete?.id} />
                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button className="button button-modal" onClick={() => {
                            setBanner(null);
                            setModalIsOpen(false);
                        }}>
                            Cancel
                        </button>
                        <button className="button button-modal" onClick={handleDelete}>Yes, Delete</button>
                    </div>
                </div>
            </Modal>

            {isMenuOpen && selectedCategory && (
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

                    {/* You will create MobileCategoryActionsMenu later */}
                    <CategoriesActionsMenu
                        recipe={selectedCategory}
                        navigate={(path: string) => navigate(path)}
                        openDeleteModal={() => openDeleteModal(selectedRecipe)}
                        closeMenu={closeMenu}
                        isClosing={isClosing}
                    />
                </>
            )}
        </div>
    );
};

export default CategoryList;
