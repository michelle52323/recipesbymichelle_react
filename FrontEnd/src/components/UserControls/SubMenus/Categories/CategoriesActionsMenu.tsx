import React from 'react';
import Icon from '../../Icons/icons';
import type { Category } from '../../../../types/Categories/Categories';
import '../../Menu/menu.css';


interface Props {
    category: Category;
    navigate: (path: string) => void;
    openDeleteModal: () => void;
    closeMenu: () => void;
    isClosing: boolean;
    showCategoryModal: boolean;
    setShowCategoryModal: (value: boolean) => void;
    isRenaming: boolean;
    setIsRenaming: (value: boolean) => void;
}

const CategoriesActionsMenu: React.FC<Props> = ({
    category,
    navigate,
    openDeleteModal,
    closeMenu,
    isClosing,
    showCategoryModal,
    setShowCategoryModal,
    isRenaming,
    setIsRenaming
}) => {

    const handleRename = (category: Category) => {
        setShowCategoryModal(true);
    }

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
                {category.name}
            </h4>
            <hr className="menu-divider" />

            <div style={{ paddingLeft: '16px', paddingRight: '16px' }}>

                {/* Rename */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            setIsRenaming(true);
                            handleRename(category);
                            closeMenu();
                        }}
                    >
                        <div className="menu-icon">
                            <Icon name="pencil" />
                        </div>
                        <div className="menu-text">Rename</div>
                    </button>
                </div>

                {/* Delete */}
                <div className="menu-item menu-item-wide">
                    <button
                        className="menu-button-link"
                        onClick={() => {
                            setIsRenaming(false);
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

export default CategoriesActionsMenu;
