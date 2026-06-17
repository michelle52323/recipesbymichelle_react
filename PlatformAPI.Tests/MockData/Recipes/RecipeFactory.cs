using System;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;

namespace PlatformAPI.Tests.MockData.Recipes
{
    public static class RecipeFactory
    {
        public static Recipe Create(
            int id = 1,
            int userId = 123,
            string name = "Sample Recipe",
            string description = "Test recipe",
            bool showAbbreviations = false,
            bool isActive = true,
            int sortOrder = 1,
            RecipeVisibility visibility = RecipeVisibility.MeOnly,
            RecipeFont font = RecipeFont.SansSerif
        )
        {
            return new Recipe
            {
                Id = id,
                Name = name,
                Description = description,
                ShowAbbreviations = showAbbreviations,
                IsActive = isActive,
                SortOrder = sortOrder,
                RecipeVisibility = visibility,
                RecipeFont = font
            };
        }

        public static List<Recipe> CreateList()
        {
            return new List<Recipe>
            {
                // Active recipes for User 123
                RecipeFactory.Create(id: 1, name: "Active Recipe 1", isActive: true, userId: 123, sortOrder: 3),
                RecipeFactory.Create(id: 2, name: "Active Recipe 2", isActive: true, userId: 123, sortOrder: 2),
                RecipeFactory.Create(id: 5, name: "Active Recipe 3", isActive: true, userId: 123, sortOrder: 5, visibility: RecipeVisibility.AllUsers),

                // Inactive recipe
                RecipeFactory.Create(id: 3, name: "Inactive Recipe", isActive: false, userId: 123),

                // Recipe belonging to another user
                RecipeFactory.Create(id: 4, name: "Someone Else's Recipe", isActive: true, userId: 999)
            };
        }
    }
}
