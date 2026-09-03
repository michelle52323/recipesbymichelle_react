import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../helpers/config';

import "./widgets.css";

function CookingTips() {

    const LOCAL_STORAGE_KEY = "cookingTipCache";
    const HOURS_24 = 24 * 60 * 60 * 1000;

    const [tip, setTip] = useState(null);
    const API_BASE = getApiBaseUrl();

    const loadCache = () => {
        try {
            const raw = localStorage.getItem(LOCAL_STORAGE_KEY);
            return raw ? JSON.parse(raw) : null;
        } catch {
            return null;
        }
    };

    const saveCache = (tipId, tipText) => {
        const now = new Date().toISOString();

        const existing = loadCache();
        const recent = existing?.recentTipIds || [];

        recent.unshift(tipId);
        if (recent.length > 15) {
            recent.pop();
        }

        const updated = {
            lastTipDateTime: now,
            recentTipIds: recent,
            lastTipText: tipText
        };

        localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(updated));
    };




    // Embedded API call
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

            const data = await response.json();
            return data; // { tipId, tipText }
        }
        catch (error) {
            console.error("Error fetching cooking tip:", error);
            return null;
        }
    };


    // Load tip on mount
    useEffect(() => {
        async function loadTip() {
            const cache = loadCache();
            const now = Date.now();

            // If cache exists and is < 24 hours old, use cached tip
            if (cache?.lastTipDateTime) {
                const last = new Date(cache.lastTipDateTime).getTime();
                const diff = now - last;

                if (diff < HOURS_24 && cache.lastTipText) {
                    setTip(cache.lastTipText);
                    return;
                }
            }



            // Cache expired or missing → fetch new tip
            const excludeIds = cache?.recentTipIds || [];
            const result = await fetchCookingTip(excludeIds);

            if (result) {
                setTip(result.tipText);
                saveCache(result.tipId, result.tipText);

            }
        }

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
