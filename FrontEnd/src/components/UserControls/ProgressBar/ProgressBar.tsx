import { useLocation } from "react-router-dom";
import './progress.css';

function ProgressBar() {
    const location = useLocation();
    const path = location.pathname.toLowerCase();

    const getClass = (keyword: string) => {
        const isMatch = path.includes(keyword.toLowerCase());
        return isMatch ? "progress-selected" : "progress-unselected";
    };

    return (
        <div className="quiz-progress-bar d-flex">
            <div className={`col-4 progress-panel ${getClass("/recipes/recipeinfo")}`}>
                1 | Basic Info
            </div>
            <div
                className={`col-4 progress-panel ${path.includes("/recipes/ingredients/")
                        ? "progress-selected"
                        : "progress-unselected"
                    }`}
            >
                2 | Ingredients
            </div>
           <div className={`col-4 progress-panel ${getClass("/recipes/steps")}`}>
                3 | Steps
            </div>
        </div>
    );
}

export default ProgressBar;