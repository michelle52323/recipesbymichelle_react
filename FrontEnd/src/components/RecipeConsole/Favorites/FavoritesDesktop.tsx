import React, { useEffect, useState } from 'react';
import { useOutletContext } from 'react-router-dom';
import Modal from 'react-modal';
import { getApiBaseUrl } from '../../../helpers/config';
import {mapApiFavoritesToFavorites} from './UtilityFunctions';
import '../../../grid-layout.css';
import SortableFavoriteItem from './SortableFavoriteItem';
const API_BASE = getApiBaseUrl();
import { TouchSensor } from '@dnd-kit/core';
import Loader from '../../UserControls/Loader/Loader';

import type {Favorite} from '../../../types/Recipe/Recipe';

import {
    DndContext,
    closestCenter,
    useSensor,
    useSensors,
    PointerSensor,
    KeyboardSensor,
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

Modal.setAppElement('#root'); // for accessibility

// interface Favorite {
//     id: number;
//     name: string;
//     description: string;
//     sortOrder: number;
   
// }

const FavoritesDesktop: React.FC = () => {

    const [favorites, setFavorites] = useState<Favorite[]>([]);

    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [favoriteToDelete, setFavoriteToDelete] = useState<{ id: number; name: string } | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    const openDeleteModal = (favorite: { id: number; name: string }) => {
        setFavoriteToDelete(favorite);
        setModalIsOpen(true);
    };

    const { setBanner } = useOutletContext<{
        setBanner: (message: string) => void;
    }>();

    const sensors = useSensors(
        useSensor(PointerSensor),
        useSensor(TouchSensor),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    useEffect(() => {
        const fetchFavorites = async () => {
            setIsLoading(true);
            
            const response = await fetch(`${API_BASE}/api/Favorites`, {
                credentials: 'include',
            });
            if (response.ok) {
                const data = await response.json();
                const cleaned = mapApiFavoritesToFavorites(data);
                //console.log("DATA:\n" + JSON.stringify(cleaned, null, 2));

                setFavorites(cleaned);
                setIsLoading(false);
            } else {
                console.error('Failed to fetch favorites');
                setIsLoading(false);
            }
        };

        fetchFavorites();
    }, []);

    const handleDragEnd = async (event: any) => {
        setBanner('');
        const { active, over } = event;
        if (active.id !== over?.id) {
            const oldIndex = favorites.findIndex(r => r.id.toString() === active.id);
            const newIndex = favorites.findIndex(r => r.id.toString() === over?.id);
            const newOrder = arrayMove(favorites, oldIndex, newIndex).map((favorite, index) => ({
                ...favorite,
                sortOrder: index + 1,
            }));

            setFavorites(newOrder);

            const response = await fetch(API_BASE + `/api/Favorites/updateSortOrder`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newOrder.map(r => ({
                    id: r.id,
                    sortOrder: r.sortOrder,
                }))),
            });

            const result = await response.json();

            if (response.ok && result.success) {
                setBanner('Favorites successfully re-ordered!');
            } else {
                setBanner('Error occurred during sorting');
            }
        }
    };

    const handleDelete = async () => {
        setBanner('');

        const response = await fetch(`${API_BASE}/api/Favorites/${favoriteToDelete?.id}`, {
            method: 'DELETE',
            credentials: 'include',
        });

        if (response.ok) {
            const refreshed = await fetch(`${API_BASE}/api/Favorites`, {
                credentials: 'include',
            });

            if (refreshed.ok) {
                const data = await refreshed.json();
                setFavorites(data);
                setBanner('Favorite successfully deleted!');
            } else {
                setBanner('Favorite deleted, but failed to reload list.');
            }
        } else {
            setBanner('Error occurred during deletion');
        }

        setModalIsOpen(false);
    };

    if (isLoading) {
        return (
            <Loader message="Loading favorites ..." />
        );
    }

    return (
        <div className="page-container w-100 pt-3">

            <div className="content-inner-desktop">

                {favorites.length === 0 && !isLoading ? (
                    <div className="empty-grid">No favorites found. Start by creating one.</div>
                ) : (
                    <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
                        <SortableContext items={favorites.map(r => r.id.toString())} strategy={verticalListSortingStrategy}>
                            
                            {/* Header */}
                            <div className="d-flex align-items-start">
                                <div className="d-flex">
                                    <div className="drag-handle-width-desktop"></div>
                                </div>

                                <div className="flex-grow-1">
                                    <div className="row">
                                        <div className="col-6 col-custom-6-12 fw-bold">Name</div>
                                        <div className="col-6 col-custom-6-0 fw-bold">Description</div>
                                        
                                    </div>
                                </div>

                                <div className="d-flex ms-3">
                                    <div className="fixed-button-icon"></div>
                                    <div className="fixed-button"></div>
                                    <div className="fixed-button"></div>
                                    <div className="fixed-button-icon"></div>
                                    <div className="fixed-button-icon"></div>
                                </div>
                            </div>

                            {/* Rows */}
                            <div className="grid-overflow-box gof-editable" id="sortable">
                                {favorites.map((favorite, i) => (
                                    <SortableFavoriteItem
                                        key={favorite.id}
                                        favorite={favorite}
                                        index={i}
                                        isMobile={false}
                                    />
                                ))}
                            </div>

                        </SortableContext>
                    </DndContext>
                )}

            </div>

            <Modal
                isOpen={modalIsOpen}
                onRequestClose={() => setModalIsOpen(false)}
                contentLabel="Confirm Delete"
                className="dialog-wrapper"
            >
                <div className="modal-header dialog-header">
                    <h5 className="modal-title">Confirm Delete</h5>
                    <button className="btn-close" onClick={() => setModalIsOpen(false)} ></button>
                </div>
                <div className="dialog-content-holder">
                    <div className="dialog-content modal-body dialog-text">
                        Are you sure you want to delete favorite "{favoriteToDelete?.name}"?
                        <input type="hidden" value={favoriteToDelete?.id} />
                    </div>

                    <div className="dialog-footer d-flex justify-content-end gap-2">
                        <button
                            className="button button-modal"
                            onClick={() => {
                                setBanner(null);
                                setModalIsOpen(false);
                            }}
                        >
                            Cancel
                        </button>
                        <button className="button button-modal" onClick={handleDelete}>Yes, Delete</button>
                    </div>
                </div>

            </Modal>

        </div>
    );
};

export default FavoritesDesktop;
