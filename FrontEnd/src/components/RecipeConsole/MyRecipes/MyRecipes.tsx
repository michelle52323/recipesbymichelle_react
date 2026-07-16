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
import CategoryList from '../Categories/CategoryList';
import type { Category } from '../../../types/Categories/Categories';
import type { UserSettings } from '../../../types/UserSettings/UserSettings';

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
    const [userSettings, setUserSettings] = useState<UserSettings | null>(null);
    const [categoryList, setCategoryList] = useState<Category[] | null>(null);
    const [showCategoriesToolbar, setShowCategoriesToolbar] = useState<boolean>(false);
    const [showCategories, setShowCategories] = useState<boolean>(false);
    const [categorySortBy, setCategorySortBy] = useState<number | null>(1);
    const [openCategory, setOpenCategory] = useState<Category | null>(null);
    const [currentView, setCurrentView] = useState<"Recipes" | "Categories" | null>(null);
    // const [categorySortBy, setCategorySortBy] = useState<number | undefined>(2);
    //console.log("Open Category : " + JSON.stringify(openCategory));
    //console.log("Current View: " + currentView);

    const [categoriesIsLoading, setCategoriesIsLoading] = useState<boolean>(true);

    //ENABLE OR DISABLE THE FEATURE
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

    useEffect(() => {
        if (auth === null) return;


        if (openCategory != null && currentView == "Recipes")
            setTitle("My Recipes > " + openCategory.name)
        else
            setTitle("My Recipes");


    }, [currentView]);

    useEffect(() => {
        if (auth === null) return;


        if (!showCategories) {
            setCurrentView("Recipes");
            setOpenCategory(null);
            setTitle("My Recipes");
        }



    }, [userSettings, showCategories]);

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
            setCategoriesIsLoading(false);
            throw new Error("Failed to fetch categories");

        }
        else {
            const data = await response.json();   // ← THIS is the important part
            setCategoryList(data);                // ← store the actual category array
            //setCategoriesIsLoading(false);

        }

    };

    useEffect(() => {

        if (categoryList && categoryList.length > 0) {
            //console.log(categoryList);

            setShowCategoriesToolbar(true);
            if (userSettings) {
                setShowCategories(userSettings.showCategories);
                setCategoriesIsLoading(false);
                setCurrentView(userSettings.showCategories ? "Categories" : "Recipes");
            }

        }
        else {
            setShowCategories(false);
        }

    }, [categoryList, userSettings]);

    // useEffect(() => {
    //     const loadCategories = async () => {
    //         await getCategories(1);
    //     };

    //     loadCategories();
    // }, []);
    useEffect(() => {
        if (!userSettings) return;

        getCategories(userSettings.categorySortBy);
    }, [userSettings]);


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

    const updateUserSettings = async (showCategories: boolean, categorySortBy: number) => {
        const endpoint = `${API_BASE}/api/Users/updateSettings${isDevUseMockLogin() ? "mock" : ""}`;

        try {
            const response = await fetch(endpoint, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    showCategories,
                    categorySortBy
                })
            });

            if (!response.ok) {
                console.error("Failed to update user settings");
            }
        } catch (err) {
            console.error("Error updating user settings", err);
        }
    };


    useEffect(() => {
        const loadSettings = async () => {
            try {
                const settings = await getUserSettings();
                setUserSettings(settings);
                setShowCategories(categoryList && categoryList.length > 0 ? settings.showCategories : false);
                setCategorySortBy(settings.categorySortBy);
            } catch (err) {
                console.error(err);
            }
        };

        loadSettings();
    }, []);

    useEffect(() => {
        if (!categoryList || categoryList.length === 0) return;

        let sorted = [...categoryList];

        if (categorySortBy === 2) {
            sorted.sort((a, b) => a.sortOrder - b.sortOrder);
        } else if (categorySortBy === 1) {
            sorted.sort((a, b) => a.name.localeCompare(b.name));
        }

        setCategoryList(sorted);
        //console.log("Sort by: " + categorySortBy);
    }, [categorySortBy]);

    // if (categoriesIsLoading) {
    //     return (<Loader message="Loading categories ..." />);
    // }

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
                {showCategoriesToolbar &&
                    userSettings &&
                    categorySortBy !== null &&
                    categoryList &&
                    (
                        <CategoriesToolbar
                            key={categorySortBy}
                            showCategories={showCategories}
                            setShowCategories={setShowCategories}
                            categorySortBy={categorySortBy}
                            setCategorySortBy={setCategorySortBy}
                            userSettings={userSettings}
                            setUserSettings={setUserSettings}
                            updateUserSettings={updateUserSettings}
                            categoriesIsLoading={categoriesIsLoading}
                            currentView={currentView}
                            setCurrentView={setCurrentView}
                            openCategory={openCategory}
                            setOpenCategory={setOpenCategory}
                        />
                    )}
                {showCategories && !categoriesIsLoading && currentView == "Categories" && (
                    <CategoryList
                        categories={categoryList}
                        setCategories={setCategoryList}
                        showCategories={showCategories}
                        setShowCategories={setShowCategories}
                        categorySortBy={categorySortBy}
                        setCategorySortBy={setCategorySortBy}
                        openCategory={openCategory}
                        setOpenCategory={setOpenCategory}
                        currentView={currentView}
                        setCurrentView={setCurrentView}
                    />
                )}

                {(!showCategories || currentView == "Recipes") && (
                    isMobileTouchDevice()
                        ? <MyRecipesMobile
                            showCategories={showCategories}
                            showCategoryToolbar={showCategoriesToolbar}
                            openCategory={openCategory}
                            setOpenCategory={setOpenCategory}
                            currentView={currentView}
                            setCurrentView={setCurrentView} />
                        : <MyRecipesDesktop
                            showCategories={showCategories}
                            showCategoryToolbar={showCategoriesToolbar}
                            openCategory={openCategory}
                            setOpenCategory={setOpenCategory}
                            currentView={currentView}
                            setCurrentView={setCurrentView} />
                )}

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
                    onCategoryAdded={() => getCategories(userSettings.categorySortBy)
                    }
                />
            )

            }
        </div>
    );
}

export default MyRecipes;
