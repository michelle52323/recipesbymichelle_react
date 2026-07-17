import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { useDndContext } from '@dnd-kit/core';
import Icon from '../../UserControls/Icons/icons';
import type { Category } from '../../../types/Categories/Categories';


interface Props {
    category: Category;
    index: number;
    isMobile: boolean;
    openDeleteModal: () => void;
    setSelectedCategory?: (category: Category) => void;
    openCategory: Category;
    setOpenCategory: (value: Category) => void;
    setIsMenuOpen?: (open: boolean) => void;
    currentView: "Recipes" | "Categories" | null;
    setCurrentView: (value: "Recipes" | "Categories" | null) => void;
    categorySortBy: "Alphabetical" | "SortOrder" | null;
}

const SortableCategoryItem: React.FC<Props> = ({
    category,
    index,
    isMobile,
    openDeleteModal,
    setSelectedCategory,
    openCategory,
    setOpenCategory,
    setIsMenuOpen,
    currentView,
    setCurrentView,
    categorySortBy
}) => {

    const navigate = useNavigate();

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({ id: category.id.toString() });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    const onCategoryClick = (cat: Category) => {
        setOpenCategory(cat);
        setCurrentView("Recipes");
    }

    const { active } = useDndContext();
    const isDragging = active?.id === category.id.toString();

    const draggingClass = isDragging ? "drag-item-tint" : "";

    return (
        <div
            ref={setNodeRef}
            style={style}
            {...attributes}
            className={`d-flex align-items-start grid-page-row grid-page-row-height-desktop sortable-container item-tint ${draggingClass}`}
        >
            <div className="d-flex">
                {categorySortBy == "SortOrder" && (
                    <div className="drag-handle drag-handle-width-desktop" {...listeners}>
                        <Icon name="drag" />
                    </div>
                )}
                {categorySortBy == "Alphabetical" && (
                    <div className="drag-handle-width-desktop" style={{ height: 50 }}
                        role="button"
                        onClick={() => onCategoryClick(category)}
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') onCategoryClick(category);
                        }}
                    >

                    </div>
                )}

            </div>

            <div className="flex-grow-1">
                <div className="row">
                    <input type="hidden" name={`MyCategoriesDto[${index}].Id`} value={category.id} />
                    <input type="hidden" name={`MyCategoriesDto[${index}].SortOrder`} value={category.sortOrder} />

                    <div
                        className="category-row d-flex align-items-start"
                        style={{ height: 50 }}
                        role="button"
                        tabIndex={0}
                        onClick={() => onCategoryClick(category)}
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') onCategoryClick(category);
                        }}
                    >
                        {isMobile ? (
                            <div className="col-12 fw-bold truncate-one-line">
                                {category.name}
                            </div>
                        ) : (
                            <div className="col-6 col-custom-6-12 fw-bold truncate-responsive">
                                {category.name}
                            </div>
                        )}

                    </div>


                </div>
            </div>

            <div className="d-flex " >

                <div
                    className="d-flex align-items-center justify-content-center"
                    style={{ width: 50, height: '100%' }}
                    onClick={() => {
                        setSelectedCategory?.(category);
                        setIsMenuOpen?.(true);
                    }}
                >
                    <div style={{ width: 25 }}></div>
                    <div style={{ width: 25 }}><Icon name="moreOptions" /></div>

                </div>


            </div>

        </div>
    );
};

export default SortableCategoryItem;
