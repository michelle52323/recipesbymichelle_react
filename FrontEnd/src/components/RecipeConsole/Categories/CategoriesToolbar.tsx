import Icon from '../../UserControls/Icons/icons';
import { getApiBaseUrl, isIOS, isAndroid } from '../../../helpers/config';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';
import type { UserSettings } from '../../../types/UserSettings/UserSettings';
import type { Category } from '../../../types/Categories/Categories';
import './categoriesToolbar.css';

interface CategoriesToolbarProps {
    showCategories: boolean;
    setShowCategories: (value: boolean) => void;
    categorySortBy: "Alphabetical" | "SortOrder" | null;
    setCategorySortBy: (value: "Alphabetical" | "SortOrder" | null) => void;
    userSettings: UserSettings;
    setUserSettings: (value: UserSettings) => void;
    updateUserSettings: (showCategories: boolean, sortBy: "Alphabetical" | "SortOrder" | null) => void;
    categoriesIsLoading: boolean;
    currentView: "Recipes" | "Categories" | null;
    setCurrentView: (value: "Recipes" | "Categories" | null) => void;
    openCategory: Category;
    setOpenCategory: (value: Category) => void;
}

function CategoriesToolbar({
    showCategories,
    setShowCategories,
    categorySortBy,
    setCategorySortBy,
    updateUserSettings,
    userSettings,
    setUserSettings,
    categoriesIsLoading,
    currentView,
    setCurrentView,
    openCategory,
    setOpenCategory
}: CategoriesToolbarProps) {

    const viewOptions = [
        { id: "0", text: "Recipes" },
        { id: "1", text: "Categories" }
    ]

    const sortOptions = [
        { id: "1", text: "A to Z" },
        { id: "2", text: "Custom" }
    ];

    const handleAllCategories = () => {
        setCurrentView("Categories");
        setOpenCategory(null);
    }

    const android = isAndroid();
    const ios = isIOS();

    const toolbarWidth = android
        ? 325
        : ios
            ? 370
            : 590;
    const dropdownWidth = android
        ? 140
        : ios
            ? 140
            : 140;

    if (categoriesIsLoading)
        return (<></>);

    return (
        <div className="content-inner-desktop">
            <div className="categories-toolbar d-flex align-items-center" style={{ height: 60, maxWidth: toolbarWidth }}>
                <div className="toolbar-left col-6">
                    {(currentView == "Categories" || openCategory == null) && (
                        <>
                            <label className="form-label-tight">View By</label>
                            <Dropdown
                                options={viewOptions}
                                selectedId={String(userSettings.showCategories ? 1 : 0)}
                                onSelect={(id) => {
                                    const newShow = id == "0" ? false : true;
                                    
                                    
                                    setShowCategories(newShow);
                                    setCurrentView(newShow ? "Categories" : "Recipes");
                                    setUserSettings(prev => ({
                                        ...prev,
                                        categorySortBy: prev.categorySortBy,
                                        showCategories: newShow
                                    }));
                                    updateUserSettings(newShow, categorySortBy);
                                }}
                                width={dropdownWidth}
                                isLoading={categoriesIsLoading}
                            /></>
                    )}
                    {
                        showCategories && currentView == "Recipes" && openCategory != null && (
                            < >
                                <div>
                                    <div
                                        onClick={handleAllCategories}
                                        role="button"
                                        tabIndex={0}
                                    >
                                        <div className="d-flex"><Icon name="leftArrow" width={20} height={20} marginTop={-1} /> Categories</div>
                                        
                                    </div>
                                </div>

                            </>
                        )
                    }

                </div>

                <div className="toolbar-right col-6 ps-3" >
                    {showCategories && currentView == "Categories" && (
                        <>
                            <label className="form-label-tight">Sort By</label>
                            <Dropdown
                                options={sortOptions}
                                selectedId={userSettings.categorySortBy == "Alphabetical" ? "1" : "2"}
                                onSelect={(id) => {
                                    //const newSort = Number(id);
                                    const newSort = id == "1" ? "Alphabetical" : "SortOrder";

                                    setCategorySortBy(newSort);
                                    setUserSettings(prev => ({
                                        ...prev,
                                        categorySortBy: newSort,
                                        showCategories: prev.showCategories
                                    }));


                                    updateUserSettings(showCategories, newSort);
                                }}
                                width={dropdownWidth}
                                isLoading={categoriesIsLoading}
                            /></>
                    )
                    }

                </div>
            </div>
        </div>




    );
}

export default CategoriesToolbar;
