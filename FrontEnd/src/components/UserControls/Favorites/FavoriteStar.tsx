import React, { useState, useEffect, useRef } from "react";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { getApiBaseUrl } from '../../../helpers/config';
import type { Step } from "../../../types/Recipe/Recipe";
import type { MeasurementUnit } from "src/types/Measurement/MeasurementType";
import { ContentEditor } from '../../UserControls/ContentEditor/ContentEditor';
import { renderStep } from '../../../helpers/measurementHelper';
import Loader from '../../UserControls/Loader/Loader';
import Icon from '../../UserControls/Icons/icons';
import './favoritesStar.css'

const API_BASE = getApiBaseUrl();

interface FavoritesStarProps {
    recipeId: string;
    onRemoved?: (recipeId: string) => void
}

function FavoritesStar({ recipeId, onRemoved }: FavoritesStarProps) {
    const [isFavorite, setIsFavorite] = useState<boolean | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    // 1. Load initial favorite state
    useEffect(() => {
        setIsLoading(true);
        async function load() {
            const res = await fetch(`${API_BASE}/api/favorites/${recipeId}`, { credentials: "include" });
            const data = await res.json();
            setIsFavorite(data.isFavorite);
            setIsLoading(false);
        }
        load();
    }, [recipeId]);

    // 2. Toggle handler
    const toggleFavorite = async () => {
        if (isFavorite === null) return;

        const newValue = !isFavorite;

        // optimistic update
        setIsFavorite(newValue);

        try {
            const method = newValue ? "POST" : "DELETE";

            const res = await fetch(`${API_BASE}/api/favorites/${recipeId}`, {
                method,
                credentials: "include"
            });

            if (!res.ok) {
                // revert on failure
                setIsFavorite(isFavorite);
            }
            else {
                if (!newValue && onRemoved) {
                    onRemoved(recipeId);
                }
            }

        } catch {
            // revert on network error
            setIsFavorite(isFavorite);
        }
    };



    // 3. Render
    if (isLoading) {
        return (
            <>
                <Loader message="" buttonReplacement={true} buttonThemed={true} />
            </>
        )
    }
    if (isFavorite === null) {
        return <Icon name="favoritesStarUnfilled" />;
    }

    return (
        <div className="cursor-pointer" onClick={toggleFavorite}>
            {isFavorite ? (
                <Icon name="favoritesStarFilled" width={26} height={26} />
            ) : (
                <Icon name="favoritesStarUnfilled" />
            )}
        </div>
    );
}


export default FavoritesStar;