import { useEffect, useState } from 'react';
import Modal from 'react-modal';
import type { Category } from '../../../types/Categories/Categories';

function CategoryAssignmentModal({
    show,
    onClose,
    onSave,
    allCategories,
    recipeCategories
}: {
    show: boolean;
    onClose: () => void;
    onSave: (selectedCategoryIds: number[]) => void;
    allCategories: Category[];
    recipeCategories: Category[]; // categories the recipe already belongs to
}) {

    const [selectedIds, setSelectedIds] = useState<number[]>([]);

    // Initialize selected categories when modal opens
    useEffect(() => {
        if (show) {
            const initial = recipeCategories?.map(c => c.id) ?? [];
            setSelectedIds(initial);
        }
    }, [show, recipeCategories]);

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

                    <div className="form-element dialog-overflow-box" >
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
                        Save
                    </button>
                </div>
            </div>
        </Modal>
    );
}

export default CategoryAssignmentModal;
