using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using PlatformAPI.DTO.Recipe;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Helpers;
using PlatformAPI.Service;


namespace PlatformAPI.RecipeImport
{
    public class DatabaseFunctions
    {
        private readonly AppDbContext _context;
        

        public DatabaseFunctions(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<object> InsertImportedRecipe(ConvertRecipeDto dto, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Compute next recipe sort order
                var maxSortOrder = await _context.Recipes
                    .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                    .MaxAsync(r => (int?)r.SortOrder) ?? 0;

                int nextSortOrder = maxSortOrder + 1;

                // 2. Create Recipe
                var recipe = new Recipe
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ShowAbbreviations = true,
                    IsActive = true,
                    SortOrder = nextSortOrder,
                    RecipeVisibility = RecipeVisibility.MeOnly,
                    RecipeFont = RecipeFont.SansSerif
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                // 3. Create UserRecipe
                var userRecipe = new UserRecipe
                {
                    UserId = userId,
                    RecipeId = recipe.Id
                };

                _context.UserRecipes.Add(userRecipe);
                await _context.SaveChangesAsync();

                // 4. Insert Ingredients + join table
                foreach (var ing in dto.Ingredients)
                {
                    var ingredient = new Ingredient
                    {
                        Quantity = float.TryParse(ing.Quantity, out var q)
                            ? q
                            : (float?)null,
                        Unit = ing.Unit,
                        Description = ing.Description,
                        Instructions = ing.Instructions,
                        SortOrder = ing.SortOrder,
                        IsActive = ing.IsActive
                    };

                    _context.Ingredients.Add(ingredient);
                    await _context.SaveChangesAsync();

                    var join = new RecipeIngredient
                    {
                        RecipeId = recipe.Id,
                        IngredientId = ingredient.Id
                    };

                    _context.RecipeIngredients.Add(join);
                }

                await _context.SaveChangesAsync();

                // 5. Insert Steps + join table
                foreach (var step in dto.Steps)
                {
                    var stepEntity = new Step
                    {
                        Description = step.Description,
                        SortOrder = step.SortOrder,
                        IsActive = step.IsActive
                    };

                    _context.Steps.Add(stepEntity);
                    await _context.SaveChangesAsync();

                    var join = new RecipeStep
                    {
                        RecipeId = recipe.Id,
                        StepId = stepEntity.Id
                    };

                    _context.RecipeSteps.Add(join);
                }

                await _context.SaveChangesAsync();

                // 6. Commit
                await transaction.CommitAsync();

                return new { success = true, recipeId = recipe.Id };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new { success = false, message = ex.ToString() };
            }
        }

    }
}
