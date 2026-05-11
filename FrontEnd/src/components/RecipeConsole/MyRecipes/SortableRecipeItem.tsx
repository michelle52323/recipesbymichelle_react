import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import Icon from '../../UserControls/Icons/icons';

interface Recipe {
    id: number;
    name: string;
    description: string;
    sortOrder: number;
}

interface Props {
    recipe: Recipe;
    index: number;
    isMobile: boolean;
    openDeleteModal: () => void;
    setSelectedRecipe?: (recipe: Recipe) => void;
    setIsMenuOpen?: (open: boolean) => void;
}

const SortableRecipeItem: React.FC<Props> = ({
    recipe,
    index,
    isMobile,
    openDeleteModal,
    setSelectedRecipe,
    setIsMenuOpen
}) => {

    const navigate = useNavigate();

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({ id: recipe.id.toString() });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    return (
        <div
            ref={setNodeRef}
            style={style}
            {...attributes}
            className="d-flex align-items-start grid-page-row grid-page-row-height-desktop sortable-container"
        >
            <div className="d-flex">
                <div className="drag-handle drag-handle-width-desktop" {...listeners}>
                    <Icon name="drag" />
                </div>
            </div>

            <div className="flex-grow-1">
                <div className="row">
                    <input type="hidden" name={`MyRecipesDto[${index}].Id`} value={recipe.id} />
                    <input type="hidden" name={`MyRecipesDto[${index}].SortOrder`} value={recipe.sortOrder} />

                    <div className="col-6 col-custom-6-12 fw-bold truncate-two-lines">
                        {recipe.name}
                    </div>

                    <div className="col-6 col-custom-6-0 truncate-two-lines-responsive">
                        {recipe.description}
                    </div>
                </div>
            </div>

            <div className="d-flex">

                {!isMobile && (
                    <>
                        {/* Take Recipe (desktop only, still respects canTake) */}
                        <div className="fixed-button-icon">
                            <button
                                className="button button-icon"
                                onClick={() => navigate(`/Recipes/RecipeInfo/${recipe.id}`)}
                            >
                                <Icon name="pencil" />
                            </button>
                        </div>

                        {/* Edit Basic Info */}
                        <div className="fixed-button">
                            <button
                                className="button button-tiny"
                                onClick={() => navigate(`/Recipes/Ingredients/${recipe.id}`)}
                            >
                                Ingredients
                            </button>
                        </div>

                        {/* Ingredients / Steps */}
                        <div className="fixed-button">
                            <button
                                className="button button-tiny"
                                onClick={() => navigate(`/Recipes/Steps/${recipe.id}`)}
                            >
                                Steps
                            </button>
                        </div>

                        {/* Review */}
                        <div className="fixed-button-icon">
                            <button
                                className="button button-icon"
                                onClick={() => navigate(`/Recipes/View/${recipe.id}`)}
                            >
                                <Icon name="eye" />
                            </button>
                        </div>

                        {/* Delete */}
                        <div className="fixed-button-icon">
                            <button
                                className="button button-icon button-icon-delete"
                                data-bs-toggle="modal"
                                data-bs-target="#deleteModal"
                                data-recipe={recipe.name}
                                data-id={recipe.id}
                                onClick={openDeleteModal}
                            >
                                <Icon name="delete" />
                            </button>
                        </div>
                    </>
                )}

                {isMobile && (
                    <div
                        onClick={() => {
                            setSelectedRecipe?.(recipe);
                            setIsMenuOpen?.(true);
                        }}
                    >
                        <Icon name="moreOptions" />
                    </div>
                )}

            </div>

        </div>
    );
};

export default SortableRecipeItem;
