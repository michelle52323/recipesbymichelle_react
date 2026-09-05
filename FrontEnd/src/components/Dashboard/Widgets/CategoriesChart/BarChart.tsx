import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Rectangle, Cell } from 'recharts';
import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import type { RecipeCategoryChartItem, RecipeCategoryChartResponse } from '../../../../types/RecipeCategoryChart/RecipeCategoryChart';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../../helpers/config';
import "../widgets.css";

const API_BASE = getApiBaseUrl();

const data = [
    { name: "Dinner", value: 7 },
    { name: "Squishies", value: 5 },
    { name: "Salads", value: 5 },
    { name: "Holidays", value: 4 },
    { name: "Soups", value: 3 },
    { name: "Lunch", value: 2 },
    { name: "Dessert", value: 3 }
];

const COLORS = [
    "#FFBB28",
    "#00C49F",
    "#0088FE",
    "#FF5C5C ",
    "#A28BFF",
    "#4CAF50",
    "#FF8042"
];


const ActiveBar = (props) => {
    return <Rectangle {...props} className="chart-highlight" />;
};





function BarChartTopFive() {

    const [categoryChart, setCategoryChart] = useState<RecipeCategoryChartResponse | null>(null);
    const navigate = useNavigate();


    const topFive = categoryChart
        ? categoryChart.categories
            .slice(0, 5)
            .map(c => ({
                name: c.categoryName,
                value: c.count
            }))
        : [];

    let categoriesCount = 0;
    let uncategorizedCount = 0;
    let totalRecipes = 0;

    if (categoryChart) {
        categoriesCount = categoryChart.categories.reduce((sum, c) => sum + c.count, 0);
        uncategorizedCount = categoryChart.uncategorizedCount;
        totalRecipes = categoriesCount + uncategorizedCount;
    }



    const fetchRecipeCategoryChart = async () => {
        const endpoint = `${API_BASE}/api/RecipeCategoryChart/get${isDevUseMockLogin() ? "Mock" : ""}`;

        try {
            const response = await fetch(endpoint, {
                method: "GET",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                throw new Error("Failed to fetch recipe category chart");
            }

            const data: RecipeCategoryChartResponse = await response.json();
            return data;
        }
        catch (error) {
            console.error("Error fetching recipe category chart:", error);
            return null;
        }
    };

    useEffect(() => {
        async function loadChart() {
            const result = await fetchRecipeCategoryChart();
            if (result) {
                setCategoryChart(result);
            }
        }

        loadChart();
    }, []);

    useEffect(() => {

    }, [categoryChart]);

    const CustomLegend = () => (
        <div
            style={{
                display: "flex",
                flexWrap: "wrap",
                rowGap: "0px",       // tighter spacing between legend lines
                columnGap: "6px",    // keep spacing between items
                justifyContent: "center",
                marginTop: "-23px",
                width: "300px",        // <-- match chart width
                marginLeft: "auto",
                marginRight: "auto"
            }}
        >

            {topFive.map((entry, index) => (
                <div key={entry.name} style={{ display: "flex", alignItems: "center", gap: "4px" }}>
                    <div
                        style={{
                            width: "12px",
                            height: "12px",
                            backgroundColor: COLORS[index],
                            borderRadius: "3px"
                        }}
                    />
                    <span>{entry.name}</span>
                </div>
            ))}
        </div>
    );

    const onCreateNewRecipe = () => {
        navigate("/recipes/recipeSettings");
    }

    const onCategorize = () => {
        navigate("/recipes/myRecipes");
    }

    const CustomToolTip = ({ active, payload }) => {
        if (active && payload && payload.length) {
            const category = payload[0].payload.name;
            const value = payload[0].value;

            return (
                <div
                    style={{
                        background: "var(--chartTooltipBackgroundColor)",
                        color: "var(--chartTooltipTextColor)",
                        padding: "6px 10px",
                        borderRadius: "6px",
                        fontSize: "14px",
                        boxShadow: "0 2px 6px rgba(0,0,0,0.4)",
                        transform: "translateY(-10px)"
                    }}
                >
                    {category}: {value}
                </div>
            );
        }
        return null;
    };

    if (categoryChart && totalRecipes === 0) {
        return (
            <div className="chart-center no-focus" style={{ textAlign: "center", paddingTop: "20px" }}>
                <div style={{ fontSize: "16px", marginBottom: "10px" }}>
                    You don’t have any recipes yet.
                </div>

                <div

                    className="widget-link widget-text widget-text-bold"
                    onClick={onCreateNewRecipe}
                    style={{ cursor: "pointer" }}
                >
                    <span>Add New Recipe →</span>
                </div>
            </div>
        );
    }

    if (categoryChart && categoriesCount === 0 && uncategorizedCount > 0) {
        return (
            <div className="chart-center no-focus" style={{ textAlign: "center", paddingTop: "20px" }}>
                <div style={{ fontSize: "16px", marginBottom: "10px" }}>
                    You don’t have any categorized recipes yet.
                </div>

                <div className="subtle-badge" style={{ marginBottom: "10px" }}>
                    Uncategorized: {uncategorizedCount}
                </div>
                <div

                    className="widget-link widget-text widget-text-bold"
                    onClick={onCategorize}
                    style={{ cursor: "pointer" }}
                >
                    <span>Categorize Recipes →</span>
                </div>
            </div>
        );
    }




    return (
        <div className="chart-center no-focus">
            <BarChart width={300} height={160} data={topFive} barSize={55}
                margin={{ top: 0, right: 5, left: -35, bottom: 0 }}
            >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" tick={false} />
                <YAxis
                    tickFormatter={(value) => Number.isInteger(value) ? value : ""}
                    tick={{
                        fill: "var(--contentTextColor)"
                    }}
                />
                <Tooltip content={CustomToolTip} cursor={
                    <Rectangle
                        style={{ fill: 'var(--chartHighlightColor)', stroke: 'none' }}
                        opacity={0.3}
                    />
                } />
                <Bar dataKey="value" >
                    {topFive.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={COLORS[index]} />
                    ))}
                </Bar>
            </BarChart>

            <CustomLegend />
            {categoryChart && (
                <div style={{
                    textAlign: "center",
                    marginTop: "3px",
                    fontSize: "14px",
                    color: "var(--contentTextColor)"
                }}>
                    <div className="subtle-badge">Uncategorized: {categoryChart.uncategorizedCount}</div>

                </div>
            )}

        </div>
    );
}

export default BarChartTopFive;
