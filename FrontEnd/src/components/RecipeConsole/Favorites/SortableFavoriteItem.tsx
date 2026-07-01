import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import Icon from '../../UserControls/Icons/icons';
import FavoritesStar from '../../../components/UserControls/Favorites/FavoriteStar';
import type { Favorite } from '../../../types/Recipe/Recipe';

// interface Favorite {
//     id: number;
//     name: string;
//     description: string;
//     sortOrder: number;
// }

interface Props {
    favorite: Favorite;
    index: number;
    isMobile: boolean;
    setSelectedFavorite?: (favorite: Favorite) => void;
    setIsMenuOpen?: (open: boolean) => void;
    onRemoved?: (recipeId: string) => void;
}

const SortableFavoriteItem: React.FC<Props> = ({
    favorite,
    index,
    isMobile,
    setSelectedFavorite,
    setIsMenuOpen,
    onRemoved
}) => {

    const navigate = useNavigate();

    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({ id: favorite.id.toString() });

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
                    <input type="hidden" name={`FavoritesDto[${index}].Id`} value={favorite.id} />
                    <input type="hidden" name={`FavoritesDto[${index}].SortOrder`} value={favorite.sortOrder} />

                    {isMobile ? (
                        <>
                            <div className="col-12 fw-bold truncate-one-line">
                                {favorite.recipe.name}
                            </div>
                            <div className="col-12 truncate-one-line">
                                {favorite.recipe.description}
                            </div>
                        </>
                    ) : (
                        <>
                            <div className="col-6 col-custom-6-12 fw-bold truncate-responsive">
                                {favorite.recipe.name}
                            </div>

                            <div className="col-6 col-custom-6-12 truncate-responsive">
                                {favorite.recipe.description}
                            </div>
                        </>
                    )}

                </div>
            </div>

            <div className="d-flex">

                {!isMobile && (
                    <>
                        {/* Take Favorite (desktop only, still respects canTake) */}
                        <div className="fixed-button-icon">
                            {favorite.isMine ? (
                                <button
                                    className="button button-icon"
                                    onClick={() => navigate(`/Recipes/RecipeInfo/${favorite.recipe.id}`)}
                                >
                                    <Icon name="pencil" marginLeft={1} marginTop={-1} />
                                </button>
                            ) : (
                                <>&nbsp;</>
                            )}
                        </div>

                        {/* Edit Basic Info */}
                        <div className="fixed-button">
                            {favorite.isMine ? (
                                <button
                                    className="button button-tiny"
                                    onClick={() => navigate(`/Recipes/Ingredients/${favorite.recipe.id}`)}
                                >
                                    Ingredients
                                </button>
                            ) : (
                                <>&nbsp;</>
                            )}
                        </div>

                        {/* Ingredients / Steps */}
                        <div className="fixed-button">
                            {favorite.isMine ? (
                                <button
                                    className="button button-tiny"
                                    onClick={() => navigate(`/Recipes/Steps/${favorite.recipe.id}`)}
                                >
                                    Steps
                                </button>
                            ) : (
                                <>&nbsp;</>
                            )}
                        </div>


                        {/* Review */}
                        <div className="fixed-button-icon">
                            <button
                                className="button button-icon"
                                onClick={() => navigate(`/Recipes/View/${favorite.recipe.id}`)}
                            >
                                <Icon name="eye" marginTop={-2} width={27} height={27} />
                            </button>
                        </div>

                        {/* Delete */}
                        <div className="fixed-button-icon">
                            <FavoritesStar recipeId={favorite.recipe.id.toString()} onRemoved={onRemoved} starLocation="grid" />

                        </div>
                    </>
                )}

                {isMobile && (
                    <div
                        onClick={() => {
                            setSelectedFavorite?.(favorite);
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

export default SortableFavoriteItem;
