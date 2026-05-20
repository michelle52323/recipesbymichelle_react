import { Favorite, FavoriteBase, RecipeBase } from "../../../types/Recipe/Recipe";

export function mapApiFavoritesToFavorites(data: any[]): Favorite[] {
    if (!Array.isArray(data)) return [];

    return data.map(item => {
        const r = item.recipe ?? {};

        return {
            // FavoriteBase fields
            id: item.id,
            userId: item.userId,
            recipeId: item.recipeId,
            sortOrder: item.sortOrder,
            isMine: item.isMine,
            isFavorite: item.isFavorite,

            // Nested RecipeBase
            recipe: {
                id: r.id,
                name: r.name ?? null,
                description: r.description ?? null,
                showAbbreviations: r.showAbbreviations ?? false,
                isActive: r.isActive ?? false,
                sortOrder: r.sortOrder ?? 0,

                recipeVisibility:
                    r.recipeVisibility === 1
                        ? "MeOnly"
                        : r.recipeVisibility === 2
                        ? "AllUsers"
                        : "AllUsers",

                recipeFont:
                    r.recipeFont === 0
                        ? "SansSerif"
                        : r.recipeFont === 1
                        ? "Serif"
                        : r.recipeFont === 2
                        ? "Handwritten"
                        : "SansSerif"
            }
        };
    });
}



