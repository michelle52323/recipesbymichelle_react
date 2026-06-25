import React from 'react';
import Icon from '../../Icons/icons';
import '../../Menu/menu.css';


interface Props {
    navigate: (path: string) => void;
    closeMenu: () => void;
    isClosing: boolean;
}

const AddRecipeActionsMenu: React.FC<Props> = ({
    navigate,
    closeMenu,
    isClosing
}) => {

    return (
        <div
            className={`menu-bottom-sheet menu-background menu-size ${isClosing ? "closing" : "opening"}`}
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
                Add New Recipe
            </h4>
            <hr className="menu-divider" />

            <div style={{ paddingLeft: '16px', paddingRight: '16px' }}>

                {/* Add Recipe Manually */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/Recipes/RecipeInfo`);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon name="pencil" />
                        </div>
                        <div className="menu-text">Add Recipe Manually</div>
                    </button>
                </div>
                {/* Add Recipe Manually */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            navigate(`/recipes/import/url`);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon name="link" />
                        </div>
                        <div className="menu-text">Import Recipe from Url</div>
                    </button>
                </div>

                

            </div>
        </div>
    );
};

export default AddRecipeActionsMenu;
