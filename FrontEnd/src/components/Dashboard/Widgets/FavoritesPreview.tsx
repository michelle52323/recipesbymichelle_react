import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getApiBaseUrl } from '../../../helpers/config';
import { mapApiFavoritesToFavorites } from '../../RecipeConsole/Favorites/UtilityFunctions';
import type { Favorite } from '../../../types/Recipe/Recipe';

import "./widgets.css";

const API_BASE = getApiBaseUrl();

function FavoritesPreview() {

    const navigate = useNavigate();
    const [favorites, setFavorites] = useState<Favorite[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchFavorites = async () => {
            try {
                const response = await fetch(`${API_BASE}/api/Favorites/`, {
                    credentials: 'include'
                });

                if (response.ok) {
                    const data = await response.json();
                    const cleaned = mapApiFavoritesToFavorites(data);

                    // Only take the top 3 (already sorted)
                    setFavorites(cleaned.slice(0, 3));
                }
            } catch (err) {
                console.error("Failed to load favorites preview", err);
            }

            setIsLoading(false);
        };

        fetchFavorites();
    }, []);

    const goToRecipe = (recipeId: number) => {
        navigate(`/Recipes/View/${recipeId}`);
    };

    const goToFavoritesPage = () => {
        navigate('/Recipes/Favorites');
    };

    return (
        <div className="widget-container">
            <h3 className="widget-title">Favorites</h3>

            {/* Loading */}
            {isLoading && (
                <p className="widget-text">Loading favorites...</p>
            )}

            {/* No favorites */}
            {!isLoading && favorites.length === 0 && (
                <p className="widget-text">
                    You have no favorites yet. Start adding your favorite recipes!
                </p>
            )}

            {/* Favorites list */}
            {!isLoading && favorites.length > 0 && (
                <div className="favorites-preview-list">

                    {favorites.map((fav, i) => (
                        <div
                            key={fav.id}
                            className="d-flex align-items-start grid-page-row grid-page-row-height-desktop item-tint favorites-preview-row"
                            role="button"
                            tabIndex={0}
                            onClick={() => goToRecipe(fav.recipe.id)}
                            onKeyDown={(e) => {
                                if (e.key === 'Enter') goToRecipe(fav.recipe.id);
                            }}
                        >
                            <div className="flex-grow-1">
                                <div className="row widget-text text-start">
                                    <div className="col-12 fw-bold truncate-one-line">
                                        {fav.recipe.name}
                                    </div>
                                    <div className="col-12 truncate-one-line">
                                        {fav.recipe.description}
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}

                </div>
            )}

            {/* Link to full favorites page */}
            <div className="favorites-preview-footer mt-2">
                
                <div

                    className="widget-link widget-text widget-text-bold"
                    onClick={goToFavoritesPage}
                    style={{ cursor: "pointer" }}
                >
                    <span>View All Favorites →</span>
                </div>
            </div>
        </div>
    );
}

export default FavoritesPreview;
