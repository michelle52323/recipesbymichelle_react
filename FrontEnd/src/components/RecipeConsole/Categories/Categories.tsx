import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../helpers/config';

import CheckAuth from '../../Account/CheckAuth';
import Icon from '../../UserControls/Icons/icons';
import Loader from '../../UserControls/Loader/Loader';
import Modal from 'react-modal';

const API_BASE = getApiBaseUrl();

function Categories({
    showAddCategory,
    setShowAddCategory,
    onCategoryAdded
}: {
    showAddCategory: boolean;
    setShowAddCategory: (value: boolean) => void;
    onCategoryAdded: () => void;
}) {

    const { setBanner } = useOutletContext<{
        setBanner: (message: string) => void;
    }>();

    const [categoryName, setCategoryName] = useState<string | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    const categoryNameRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (showAddCategory) {
            setTimeout(() => {
                categoryNameRef.current?.focus();
            }, 0);
        }
    }, [showAddCategory]);

    const handleAdd = async () => {
        // Validate
        if (!categoryName || categoryName.trim().length === 0) {
            setErrorMessage("Please enter a category name.");
            return;
        }

        try {
            setErrorMessage(null); // clear previous errors

            const result = await addCategory(categoryName.trim());

            onCategoryAdded(); 
            // Close modal
            setShowAddCategory(false);

            // Optionally clear input
            setCategoryName(null);

            // TODO: refresh categories list here if needed

        } catch (err) {
            setErrorMessage("Unable to add category. Please try again.");
            console.error(err);
        }
    };


    const addCategory = async (name: string) => {
        const endpoint = `${API_BASE}/api/Categories/add${isDevUseMockLogin() ? "mock" : ""}`;
        const response = await fetch(endpoint, {
            method: "POST",
            credentials: "include",   // ← always include credentials
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name })
        });

        if (!response.ok) {
            throw new Error("Failed to add category");
            setBanner('Error adding category');
        }else{
            setBanner('Category added successfully!');
        }

        return await response.json();
    };



    return (
        <>
            <Modal
                isOpen={showAddCategory}
                onRequestClose={() => setShowAddCategory(false)}

                contentLabel="Confirm Delete"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">New Category</h5>
                    <button className="btn-close" onClick={() => setShowAddCategory(false)} ></button>
                </div>
                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        <label className="form-label-tight">Category Name</label>
                        <div className="form-element">
                            <input
                                ref={categoryNameRef}
                                type="text"
                                className="form-control textbox textbox-large textbox-text"
                                onChange={e => setCategoryName(e.target.value)}
                            // value={profile.middleName}
                            // onChange={e => setProfile(prev => ({ ...prev, middleName: e.target.value }))}

                            />
                        </div>
                        <div className="error-message-placeholder-height">
                            {errorMessage && (
                                <div className="error-message">
                                    {errorMessage}
                                </div>
                            )}
                        </div>

                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button
                            className="button button-modal"
                            onClick={() => {
                                //setBanner(null);
                                setShowAddCategory(false);
                            }}
                        >
                            Cancel
                        </button>
                        <button className="button button-modal" onClick={handleAdd} >Add</button>
                    </div>
                </div>

            </Modal>
        </>
    );
}

export default Categories;