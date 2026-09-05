export interface RecipeCategoryChartItem {
    categoryId: number;
    categoryName: string;
    count: number;
}

export interface RecipeCategoryChartResponse {
    categories: RecipeCategoryChartItem[];
    uncategorizedCount: number;
}
