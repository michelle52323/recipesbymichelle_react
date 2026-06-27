
import React, { useEffect, useState, } from "react";
import { useNavigate, useOutletContext, useLocation } from "react-router-dom";
import { getApiBaseUrl, isMobileTouchDevice } from "../../../helpers/config";
import Loader from "../../UserControls/Loader/Loader";
import Icon from "../../UserControls/Icons/icons";
import FavoritesStar from "../../UserControls/Favorites/FavoriteStar";
import "../../../grid-layout.css";

const API_BASE = getApiBaseUrl();

interface OutletContextType {
    setTitle: (title: string) => void;
}

interface SearchResult {
    recipeId: number;
    name: string;
    description: string;
    isFavorite: boolean;
}

export default function SearchOthers() {
    const navigate = useNavigate();
    const location = useLocation();

    const { setTitle } = useOutletContext<OutletContextType>();

    //const [query, setQuery] = useState("");
    const initialSearchTerm = location.state?.searchTerm;

    const [query, setQuery] = useState(
        initialSearchTerm === "<ALL/>" ? "" : (initialSearchTerm ?? "")
    );


    const [results, setResults] = useState<SearchResult[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [hasSearched, setHasSearched] = useState(false);


    useEffect(() => {
        setTitle("Search Others' Recipes");
    }, [setTitle]);

    useEffect(() => {
        if (initialSearchTerm !== undefined) {
            performSearch();
        }
    }, []);

    //console.log("VIEW QUERY: " + query);

    const performSearch = async () => {

        setHasSearched(true);

        setIsLoading(true);

        const res = await fetch(
            `${API_BASE}/api/search/others?query=${encodeURIComponent(query)}`,
            { credentials: "include" }
        );

        if (res.ok) {
            const data = await res.json();
            setResults(data);
        }

        setIsLoading(false);
    };

    return (
        <div className="page-container w-100 pt-3">
            <div className="content-inner-desktop">

                {/* Search Bar */}
                <div className="d-flex pb-3">
                    <input
                        type="text"
                        className="form-control textbox textbox-large textbox-text"
                        placeholder="Search recipes..."
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        onKeyDown={(e) => e.key === "Enter" && performSearch()}
                    />

                    <div className="ps-3">
                        <button
                            className="button button-icon button-icon-search"
                            onClick={performSearch}
                        >
                            <span className="btn-text">
                                <div className="margin-3"><Icon name="search" /></div>
                            </span>
                        </button>
                    </div>
                </div>




                {isLoading && <Loader message="Searching..." />}

                {hasSearched && !isLoading && results.length === 0 && (
                    <div className="empty-grid">No recipes found.</div>
                )}

                {/* Results */}
                {/* <div className={isMobileTouchDevice() ? "content-holder-mobile" : "content-holder-desktop"}> */}
                <div className="">
                    <div className="grid-overflow-box gof-search">
                        {results.map((r) => (
                            <SearchRow
                                key={r.recipeId}
                                recipe={r}
                                navigate={navigate}
                                query={query}
                                onFavoriteRemoved={(id) => {
                                    setResults((prev) =>
                                        prev.map((x) =>
                                            x.recipeId === id ? { ...x, isFavorite: false } : x
                                        )
                                    );
                                }}
                            />
                        ))}
                    </div>

                </div>
            </div>
        </div>

    );
}

interface RowProps {
    recipe: SearchResult;
    navigate(to: string, options?: { state?: any })

    onFavoriteRemoved: (id: number) => void;
    query: string;
}

const SearchRow: React.FC<RowProps> = ({ recipe, navigate, onFavoriteRemoved, query }) => {
    const isMobile = isMobileTouchDevice();

    return (
        <div className="grid-page-row grid-page-row-height-desktop d-flex align-items-start mb-2 p-2 sortable-border">

            {/* Name + Description */}
            <div className="flex-grow-1">
                <div className="fw-bold truncate-one-line">{recipe.name}</div>
                <div className="truncate-one-line">{recipe.description}</div>
            </div>

            {/* Actions */}
            <div className="d-flex align-items-center ms-2">

                {/* View */}
                <button
                    onClick={() => navigate(`/Recipes/View/${recipe.recipeId}`, {
                        state: { searchTerm: query }
                    })}
                    className="button button-icon me-2"
                >
                    {isMobile ? (
                        <div className="margin-1">
                            <Icon name="eye" width={31} height={31} />
                        </div>
                    ) : (
                        <div className="margin-4">
                            <div style={{ marginLeft: -1, marginTop: -1 }} ><Icon name="eye" width={27} height={27} /></div>
                        </div>
                    )}
                </button>


                {/* Favorite Star (fixed width wrapper) */}
                <div style={{ width: "32px", display: "flex", justifyContent: "center" }}>
                    <FavoritesStar
                        recipeId={recipe.recipeId.toString()}
                        onRemoved={(id) => onFavoriteRemoved(Number(id))}
                    />
                </div>

            </div>

        </div>
    );
};
