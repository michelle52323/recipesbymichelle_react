using PlatformAPI.Models.Recipe;

namespace PlatformAPI.Tests.MockData.UserRecipes
{
    public static class UserRecipeFactory
    {
        public static UserRecipe Create(
            int userId = 123,
            int recipeId = 1)
        {
            return new UserRecipe
            {
                UserId = userId,
                RecipeId = recipeId
            };
        }

        public static List<UserRecipe> CreateList()
        {
            return new List<UserRecipe>
            {
                // Links for User 123
                UserRecipeFactory.Create(userId: 123, recipeId: 1),
                UserRecipeFactory.Create(userId: 123, recipeId: 2),
                UserRecipeFactory.Create(userId: 123, recipeId: 3),
                UserRecipeFactory.Create(userId: 123, recipeId: 5),

                // Link for User 999
                UserRecipeFactory.Create(userId: 999, recipeId: 4)
            };
        }
    }
}
