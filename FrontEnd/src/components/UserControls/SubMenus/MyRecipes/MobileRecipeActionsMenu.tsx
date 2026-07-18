import React from 'react';
import Icon from '../../Icons/icons';
import type { Category } from '../../../../types/Categories/Categories';
import '../../Menu/menu.css';

interface Recipe {
    id: number;
    name: string;
    description: string;
    sortOrder: number;
    categories?: Category[];
}

interface Props {
    recipe: Recipe;
    navigate: (path: string) => void;
    openDeleteModal: () => void;
    closeMenu: () => void;
    isClosing: boolean;
    handleOpenAssignCategories: () => void;
}

const MobileRecipeActionsMenu: React.FC<Props> = ({
    recipe,
    navigate,
    openDeleteModal,
    closeMenu,
    isClosing,
    handleOpenAssignCategories
}) => {

    return (
        <div
            className={`mobile-bottom-sheet menu-background menu-size ${isClosing ? "closing" : "opening"}`}
            style={{
                position: 'fixed',
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
                {recipe.name}
            </h4>
            <hr className="menu-divider" />

            <div style={{ paddingLeft: '16px', paddingRight: '16px' }}>

                {/* Edit Basic Info */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/Recipes/RecipeInfo/${recipe.id}`);
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
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/Recipes/Ingredients/${recipe.id}`);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">🧂</div>
                        <div className="menu-text">Ingredients</div>
                    </button>
                </div>

                {/* Steps */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/Recipes/Steps/${recipe.id}`);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">➤</div>
                        <div className="menu-text">Steps</div>
                    </button>
                </div>

                {/* Assign Categories */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            handleOpenAssignCategories();
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon marginTop={-5} name="folder" width={21} height={21} />
                        </div>
                        <div className="menu-text">Assign Categories</div>
                    </button>
                </div>

                {/* Delete */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            openDeleteModal();
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon marginTop={-5} name="delete" width={21} height={21} />
                        </div>
                        <div className="menu-text">Delete</div>
                    </button>
                </div>

            </div>
        </div>
    );
};

export default MobileRecipeActionsMenu;
