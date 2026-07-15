import { useEffect, useState } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../helpers/config';
import CheckAuth from '../../Account/CheckAuth';
import MyRecipesMobile from './MyRecipesMobile';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import Icon from '../../UserControls/Icons/icons';
import Loader from '../../UserControls/Loader/Loader';
import MyRecipesDesktop from './MyRecipesDesktop';
import AddRecipeActionsMenu from '../../UserControls/SubMenus/MyRecipes/AddRecipeActionsMenu';
import Categories from '../Categories/Categories';
import CategoriesToolbar from '../Categories/CategoriesToolbar';
import type { Category } from '../../../types/Categories/Categories';

const API_BASE = getApiBaseUrl();

// ---- Types ----
interface AuthResult {
    auth: boolean;
    claims: Record<string, string>;
}

interface OutletContextType {
    setTitle: (title: string) => void;
    setBanner: (banner: string) => void;
}

function MyRecipes() {
    const navigate = useNavigate();
    const { setTitle, setBanner } = useOutletContext<OutletContextType>();
    const [auth, setAuth] = useState<AuthResult | null>(null);
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isClosing, setIsClosing] = useState(false);

    const [showAddCategory, setShowAddCategory] = useState(false);
    const [categoryList, setCategoryList] = useState<Category[] | null>(null);
    const [showCategoriesToolbar, setShowCategoriesToolbar] = useState<boolean>(false);
    const [showCategories, setShowCategories] = useState<boolean>(false);
    const [categorySortBy, setCategorySortBy] = useState<number | null>(1);
    const categoriesFeatureEnabled = false;

    // Cleanup banner on unmount
    useEffect(() => {
        return () => {
            setBanner('');
        };
    }, [setBanner]);

    const closeMenu = () => {
        setIsClosing(true);

        setTimeout(() => {
            setIsMenuOpen(false);
            setIsClosing(false);
        }, 150);
    };

    useEffect(() => {
        window.scrollTo(0, 0);
    }, []);




    // Load auth
    useEffect(() => {
        async function hydrateAuth() {
            const result = await CheckAuth();
            setAuth(result);
        }
        hydrateAuth();
    }, []);

    // Redirect logic
    useEffect(() => {
        if (auth === null) return;

        if (!auth.auth) {
            navigate("/signin");
        } else {
            setTitle("My Recipes");
        }
    }, [auth, navigate, setTitle]);

    const getCategories = async (sortBy: number) => {
        const endpoint = `${API_BASE}/api/Categories/list${isDevUseMockLogin() ? "mock" : ""}?sortBy=${sortBy}`;
        const response = await fetch(endpoint, {
            method: "GET",
            credentials: "include", // ← always include credentials
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error("Failed to fetch categories");
        }
        else {
            const data = await response.json();   // ← THIS is the important part
            setCategoryList(data);                // ← store the actual category array

        }

    };

    useEffect(() => {
        if (categoryList && categoryList.length > 0) {
            //console.log(categoryList);
            setShowCategoriesToolbar(true);
        }

    }, [categoryList]);

    useEffect(() => {
        const loadCategories = async () => {
            await getCategories(1);
        };

        loadCategories();
    }, []);

    const getUserSettings = async () => {
        const endpoint = `${API_BASE}/api/Users/settings${isDevUseMockLogin() ? "mock" : ""}`;
        const response = await fetch(endpoint, {
            method: "GET",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error("Failed to fetch user settings");
        }

        const data = await response.json();
        return data; // { showCategories: boolean, categorySortBy: number }
    };

    useEffect(() => {
        const loadSettings = async () => {
            try {
                const settings = await getUserSettings();
                setShowCategories(settings.showCategories);
                setCategorySortBy(settings.categorySortBy);
            } catch (err) {
                console.error(err);
            }
        };

        loadSettings();
    }, []);



    if (auth === null) {
        return (
            <div>
                <Loader message="Loading recipes ..." />
            </div>
        );
    }

    if (!auth.auth) return null;

    return (
        <div className="page-container w-100">
            <div className={isMobileTouchDevice() ? "content-holder-mobile" : "content-holder-desktop"}>
                {showCategoriesToolbar && (
                    <CategoriesToolbar
                        showCategories={showCategories}
                        setShowCategories={setShowCategories}
                        categorySortBy={categorySortBy}
                        setCategorySortBy={setCategorySortBy} />
                )}
                {isMobileTouchDevice() ? <MyRecipesMobile showCategoryToolbar={showCategoriesToolbar} />
                    :
                    <MyRecipesDesktop showCategoryToolbar={showCategoriesToolbar} />}
            </div>

            <ButtonGrid
                buttons={[
                    categoriesFeatureEnabled && {
                        text: "Category",
                        onClick: () => setShowAddCategory(true),
                        icon: <Icon name="add" />,
                        type: "button",
                        mobileSlot: 2,
                        desktopSlot: 3
                    },
                    {
                        text: "Recipe",
                        onClick: () => setIsMenuOpen(true),
                        icon: <Icon name="add" />,
                        type: "button",
                        mobileSlot: 3,
                        desktopSlot: 5
                    }
                ]}
            />

            {isMenuOpen && (
                <>
                    <div
                        className="mobile-menu-backdrop"
                        onClick={closeMenu}
                        style={{
                            position: 'fixed',
                            inset: 0,
                            background: 'rgba(0,0,0,0.4)',
                            zIndex: 9998
                        }}
                    ></div>

                    <AddRecipeActionsMenu
                        navigate={(path: string) => navigate(path)}
                        closeMenu={closeMenu}
                        isClosing={isClosing}
                    />
                </>
            )}

            {showAddCategory && (
                <Categories
                    showAddCategory={showAddCategory}
                    setShowAddCategory={setShowAddCategory}
                    onCategoryAdded={() => getCategories(1)} />
            )

            }
        </div>
    );
}

export default MyRecipes;
