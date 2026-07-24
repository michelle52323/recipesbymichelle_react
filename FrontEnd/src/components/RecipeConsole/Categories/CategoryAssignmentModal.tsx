import { useEffect, useState } from 'react';
import Modal from 'react-modal';
import Icon from '../../UserControls/Icons/icons';
import Categories from '../../RecipeConsole/Categories/Categories'
import type { Category } from '../../../types/Categories/Categories';

interface Recipe {
    id: number;
    name: string
    categories?: Category[];
}

function CategoryAssignmentModal({
    show,
    onClose,
    onSave,
    allCategories,
    recipeCategories,
    recipe = null,
    openModal = null,
    setOpenModal,
    newlyAddedCategory = null,
    setNewlyAddedCategory
}: {
    show: boolean;
    onClose: () => void;
    onSave: (selectedCategoryIds: number[]) => void;
    allCategories: Category[];
    recipeCategories: Category[]; // categories the recipe already belongs to
    recipe?: Recipe;
    openModal?: "AddCategory" | "AssignCategory" | null;
    setOpenModal?: (value: "AddCategory" | "AssignCategory" | null) => void;
    newlyAddedCategory?: Category;
    setNewlyAddedCategory?: (value: Category) => void;
}) {

    const [selectedIds, setSelectedIds] = useState<number[]>([]);


    // Initialize selected categories when modal opens
    useEffect(() => {
        if (show) {
            const initial = recipeCategories?.map(c => c.id) ?? [];
            setSelectedIds(initial);
        }
    }, [show, recipeCategories]);

    useEffect(() => {
        if (
            newlyAddedCategory &&
            newlyAddedCategory.id != null &&
            !selectedIds.includes(newlyAddedCategory.id)
        ) {
            setSelectedIds(prev => [...prev, newlyAddedCategory.id]);
        }
    }, [newlyAddedCategory]);


    const toggleCategory = (id: number) => {
        setSelectedIds(prev =>
            prev.includes(id)
                ? prev.filter(x => x !== id)
                : [...prev, id]
        );
    };

    const handleSave = () => {
        onSave(selectedIds);
        onClose();
    };

    const handleAddCategory = () => {
        setOpenModal("AddCategory");
        //onClose();

    }

    // if (isAddCategoryModalOpen)
    //     return (<Categories />);
    //console.log("DATA:",recipe);
    return (
        <Modal
            isOpen={show}
            onRequestClose={onClose}
            contentLabel="Assign Categories"
            className="dialog-wrapper"
        >
            <div className="modal-header dialog-header">
                <h5 className="modal-title">Assign Categories</h5>
                <button className="btn-close" onClick={onClose}></button>
            </div>

            <div className="dialog-content-holder">
                <div className="dialog-content modal-body dialog-text">

                    <label className="form-label-tight dialog-label pb-2">
                        Select categories for this recipe
                    </label>

                    {recipe && (
                        <label
                            className="form-label-tight dialog-label pb-2"
                            style={{ width: "100%" }}
                        >
                            <span className="ps-3">{recipe.name}</span>
                        </label>
                    )}


                    <div className="form-element dialog-overflow-box subtle-border pt-3 rounded-1" >
                        <div className="pb-2">
                            <div className="d-flex button-no-fill"
                                onClick={handleAddCategory}
                            ><Icon name="add" />&nbsp;Category</div>
                        </div>
                        {allCategories.map(cat => (
                            <div key={cat.id} className="checkbox-line">
                                <input
                                    type="checkbox"
                                    className="checkbox"
                                    checked={selectedIds.includes(cat.id)}
                                    onChange={() => toggleCategory(cat.id)}
                                />
                                <span className="checkbox-label">{cat.name}</span>
                            </div>
                        ))}
                    </div>

                </div>

                <div className="dialog-footer d-flex justify-content-end gap-2">
                    <button className="button button-modal" onClick={onClose}>
                        Cancel
                    </button>

                    <button className="button button-modal" onClick={handleSave}>
                        Assign
                    </button>
                </div>
            </div>
        </Modal>
    );
}

export default CategoryAssignmentModal;
