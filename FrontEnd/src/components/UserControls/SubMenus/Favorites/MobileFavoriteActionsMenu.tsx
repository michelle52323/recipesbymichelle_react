import React from 'react';
import Icon from '../../Icons/icons';
import type { Favorite } from '../../../../types/Recipe/Recipe';
import FavoritesStar from '../../Favorites/FavoriteStar';

import '../../Menu/menu.css';

interface Recipe {
    id: number;
    name: string;
    description: string;
    sortOrder: number;
    subject: {
        id: number;
        description: string;
    };
}

interface Props {
    favorite: Favorite;
    navigate: (path: string) => void;
    openDeleteModal: () => void;
    closeMenu: () => void;
    isClosing: boolean;
    onRemoved?: (recipeId: string) => void;
}

const MobileFavoriteActionsMenu: React.FC<Props> = ({
    favorite,
    navigate,
    openDeleteModal,
    closeMenu,
    isClosing,
    onRemoved
}) => {

    return (
        <div
            className={`mobile-bottom-sheet menu-background ${isClosing ? "closing" : "opening"}`}
            style={{
                position: 'fixed',
                left: 0,
                right: 0,
                bottom: 0,
                borderTopLeftRadius: '16px',
                borderTopRightRadius: '16px',
                boxShadow: '0 -4px 20px rgba(0,0,0,0.2)',
                paddingTop: '16px',
                paddingBottom: '16px',
                zIndex: 9999
            }}
        >

            <h4 className="menu-header" style={{ paddingLeft: '16px', paddingRight: '16px' }}>
                {favorite.recipe.name}
            </h4>
            <hr className="menu-divider" />

            <div style={{ paddingLeft: '16px', paddingRight: '16px' }}>


                {favorite.isMine ? (
                    <>
                        {/* Edit Basic Info */}
                        <div className="menu-item">
                            <button
                                className="menu-button-link"
                                onClick={() => {
                                    navigate(`/Recipes/RecipeInfo/${favorite.recipe.id}`);
                                    closeMenu();
                                }}
                            >
                                <div className="menu-icon">
                                    <Icon name="pencil" />
                                </div>
                                <div className="menu-text">Edit Basic Info</div>
                            </button>
                        </div>

                        {/* Ingredients */}
                        <div className="menu-item">
                            <button
                                className="menu-button-link"
                                onClick={() => {
                                    navigate(`/Recipes/Ingredients/${favorite.recipe.id}`);
                                    closeMenu();
                                }}
                            >
                                <div className="menu-icon">🧂</div>
                                <div className="menu-text">Ingredients</div>
                            </button>
                        </div>

                        {/* Steps */}
                        <div className="menu-item">
                            <button
                                className="menu-button-link"
                                onClick={() => {
                                    navigate(`/Recipes/Steps/${favorite.recipe.id}`);
                                    closeMenu();
                                }}
                            >
                                <div className="menu-icon">➤</div>
                                <div className="menu-text">Steps</div>
                            </button>
                        </div>
                    </>
                ) : (
                    <></>
                )}


                {/* Review */}
                <div className="menu-item">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/Recipes/View/${favorite.recipe.id}`);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon name="eye" marginTop={2} width={28} height={28} />
                        </div>
                        <div className="menu-text">View</div>
                    </button>
                </div>

                {/* Delete */}
                <div className="menu-item">
                    <div className="menu-button-link">
                        <FavoritesStar recipeId={favorite.recipe.id.toString()}
                            onRemoved={(id) => {
                                if (onRemoved) onRemoved(id);   // remove from list
                                closeMenu();                    // close the sheet
                            }}
                            starLocation="menu"
                            />
                        <div className="menu-text">Remove from Favorites</div>
                    </div>
                </div>

            </div>
        </div>
    );
};

export default MobileFavoriteActionsMenu;
