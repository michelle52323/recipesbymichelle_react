import Icon from '../../UserControls/Icons/icons';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';

interface CategoriesToolbarProps {
    showCategories: boolean;
    setShowCategories: (value: boolean) => void;
    categorySortBy: number;
    setCategorySortBy: (value: number) => void;
}

function CategoriesToolbar({
    showCategories,
    setShowCategories,
    categorySortBy,
    setCategorySortBy
}: CategoriesToolbarProps) {

    const sortOptions = [
        { id: "1", text: "Sort Order" },
        { id: "2", text: "ABC" }
    ];

    return (
        <div className="content-inner-desktop">
            <div className="categories-toolbar d-flex align-items-center" style={{ height: 50 }}>
                <div className="toolbar-left">
                    <button
                        className="button button-large button-shrink-text-large"
                        onClick={() => setShowCategories(!showCategories)}
                    >
                        {showCategories ? "Hide Categories" : "Show Categories"}
                    </button>
                </div>

                <div className="toolbar-right d-flex align-items-center gap-2">
                    <label className="form-label-tight">Sort By</label>

                    <Dropdown
                        options={sortOptions}
                        selectedId={String(categorySortBy)}
                        onSelect={(id) => setCategorySortBy(Number(id))}
                        width={140}
                    />
                </div>
            </div>
        </div>


    );
}

export default CategoriesToolbar;
