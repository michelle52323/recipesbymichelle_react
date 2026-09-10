import { useEffect, useState } from 'react';
import { isDevUseMockLogin, getApiBaseUrl } from '../../../helpers/config';

import "./widgets.css";

function CookingTips() {

    const LOCAL_STORAGE_KEY = "cookingTipCache";
    const API_BASE = getApiBaseUrl();

    const [tip, setTip] = useState(null);

    // Load cache
    const loadCache = () => {
        try {
            const raw = localStorage.getItem(LOCAL_STORAGE_KEY);
            return raw ? JSON.parse(raw) : null;
        } catch {
            return null;
        }
    };

    // Save cache with new daily fields
    const saveCache = (tipId, tipText, tipDay) => {
        const existing = loadCache();
        const recent = existing?.recentTipIds || [];

        recent.unshift(tipId);
        if (recent.length > 15) {
            recent.pop();
        }

        const updated = {
            lastTipCalendarDate: tipDay,   // NEW FIELD
            lastTipText: tipText,
            recentTipIds: recent
        };

        localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(updated));
    };

    // Determine the "tip day" based on 4 AM cutoff
    const getTipDay = () => {
        const now = new Date();
        const local = new Date(now);

        // Before 4 AM → treat as previous day
        if (local.getHours() < 4) {
            local.setDate(local.getDate() - 1);
        }

        return local.toISOString().split("T")[0]; // "YYYY-MM-DD"
    };

    // API call
    const fetchCookingTip = async (excludeIds) => {
        const endpoint = `${API_BASE}/api/CookingTips/getTip${isDevUseMockLogin() ? "Mock" : ""}`;

        const requestBody = {
            excludeIds: excludeIds || []
        };

        try {
            const response = await fetch(endpoint, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });

            if (!response.ok) {
                throw new Error("Failed to fetch cooking tip");
            }

            return await response.json(); // { tipId, tipText }
        }
        catch (error) {
            console.error("Error fetching cooking tip:", error);
            return null;
        }
    };

    // Load tip on mount — DAILY RESET VERSION
    useEffect(() => {
        const loadTip = async () => {
            const cache = loadCache();
            const todayTipDay = getTipDay();

            // If cache matches today's tip day → reuse cached tip
            if (cache?.lastTipCalendarDate === todayTipDay && cache.lastTipText) {
                setTip(cache.lastTipText);
                return;
            }

            // Otherwise → fetch new tip
            const excludeIds = cache?.recentTipIds || [];
            const result = await fetchCookingTip(excludeIds);

            if (result) {
                setTip(result.tipText);
                saveCache(result.tipId, result.tipText, todayTipDay);
            }
        };

        loadTip();
    }, []);

    return (
        <div className="widget-container">
            <h3 className="widget-title">Cooking Tip of the Day</h3>
            <p className="widget-text widget-text-bold">{tip || "Loading..."}</p>
        </div>
    );
}

export default CookingTips;
