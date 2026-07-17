import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../helpers/config';
import type { Category } from '../../../types/Categories/Categories';

import CheckAuth from '../../Account/CheckAuth';
import Icon from '../../UserControls/Icons/icons';
import Loader from '../../UserControls/Loader/Loader';
import Modal from 'react-modal';

const API_BASE = getApiBaseUrl();

function Categories({
    showCategoryModal,
    setShowCategoryModal,
    onCategorySaved,
    mode = "add",
    categoryToEdit = null,
    isRenaming = false,
    setIsRenaming,
    setSelectedCategory
}: {
    showCategoryModal: boolean;
    setShowCategoryModal: (value: boolean) => void;
    onCategorySaved: () => void;
    mode?: "add" | "edit" | null;
    categoryToEdit?: Category | null;
    isRenaming?: boolean;
    setIsRenaming?: (value: boolean) => void;
    setSelectedCategory?: (value: Category | null) => void;

}) {

    const { setBanner } = useOutletContext<{
        setBanner: (message: string) => void;
    }>();

    const [categoryName, setCategoryName] = useState<string | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    const categoryNameRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (showCategoryModal) {
            setTimeout(() => {
                categoryNameRef.current?.focus();
            }, 0);

            if (mode === "edit" && categoryToEdit) {
                setCategoryName(categoryToEdit.name);
            } else {
                setCategoryName("");
            }
        }
    }, [showCategoryModal, mode, categoryToEdit]);

    const handleSave = async () => {
        if (!categoryName || categoryName.trim().length === 0) {
            setErrorMessage("Please enter a category name.");
            return;
        }

        try {
            setErrorMessage(null);

            if (mode === "add") {
                await addCategory(categoryName.trim());
            } else if (mode === "edit" && categoryToEdit) {
                await renameCategory(categoryToEdit.id, categoryName.trim());
            }

            onCategorySaved();
            setShowCategoryModal(false);
            setCategoryName(null);

        } catch (err) {
            setErrorMessage("Unable to save category. Please try again.");
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
        } else {
            setBanner('Category added successfully!');
        }

        return await response.json();
    };

    const renameCategory = async (id: number, name: string) => {
        const endpoint = `${API_BASE}/api/Categories/rename${isDevUseMockLogin() ? "mock" : ""}`;
        const response = await fetch(endpoint, {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ id, name })
        });

        if (!response.ok) {
            setBanner("Error renaming category");
            throw new Error("Failed to rename category");
        } else {
            setSelectedCategory(null);
            setIsRenaming(false);
            setBanner("Category renamed successfully!");
        }


        return await response.json();
    };




    return (
        <>
            <Modal
                isOpen={showCategoryModal}
                onRequestClose={() => {
                    if (mode == "edit") {
                        setSelectedCategory(null);
                        setIsRenaming(false);
                    }

                    setShowCategoryModal(false);
                }}

                contentLabel="Confirm Delete"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">{mode == "add" ? "New Category" : "Rename Category"}</h5>
                    <button className="btn-close" onClick={() => {
                        if (mode == "edit") {
                            setSelectedCategory(null);
                            setIsRenaming(false);
                        }

                        setShowCategoryModal(false);
                    }} ></button>
                </div>
                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        <label className="form-label-tight dialog-label">Category Name</label>
                        <div className="form-element">
                            <input
                                ref={categoryNameRef}
                                type="text"
                                className="form-control textbox textbox-large textbox-text"
                                value={categoryName}
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
                                if (mode == "edit") {
                                    setSelectedCategory(null);
                                    setIsRenaming(false);
                                }

                                setShowCategoryModal(false);
                            }}
                        >
                            Cancel
                        </button>
                        <button className="button button-modal" onClick={handleSave} >Add</button>
                    </div>
                </div>

            </Modal>
        </>
    );
}

export default Categories;