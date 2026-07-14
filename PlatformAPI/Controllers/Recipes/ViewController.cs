using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Data;
using Microsoft.AspNetCore.Authorization;
using PlatformAPI.Enums;
using PlatformAPI.DTO.Recipe;
using PlatformAPI.Helpers;
using PlatformAPI.Enums;

namespace PlatformAPI.Controllers.Recipes
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ViewRecipeDTO>> GetRecipe(int id)
        {
            var userId = User?
            .Claims
            .FirstOrDefault(c => c.Type == "UserId")
            ?.Value;

            // Load the recipe itself
            var recipe = await _context.Recipes
                .Include(r => r.UserRecipe)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (recipe == null)
                return NotFound();

            //Load the measurement system that belongs to the user 
            //who created the recipe
            MeasurementSystem measurementSystem = await GetMeasurementSystemByUser(recipe.UserRecipe.UserId);

            //Load fraction lookup table
            List<FractionDecimal>? fractionTable = null;

            if (measurementSystem == MeasurementSystem.Imperial)
            {
                var fractionDb = new FractionHelperDB(_context);
                fractionTable = await fractionDb.LoadFractionTableAsync();
            }

            //Load unit lookup table
            var unitDB = new UnitDB(_context);
            List<Unit> unitTable = await unitDB.LoadUnitTableAsync();

            // Load ingredients for this recipe
            var ingredients = await _context.Ingredients
                .Include(i => i.RecipeIngredient)
                .Where(i => i.RecipeIngredient.RecipeId == id && i.IsActive)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            // Load steps for this recipe
            var steps = await _context.Steps
                .Include(s => s.RecipeStep)
                .Where(s => s.RecipeStep.RecipeId == id && s.IsActive)
                .OrderBy(s => s.SortOrder)
                .ToListAsync();

            // Map ingredients to DTOs
            var ingredientDtos = ingredients
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Quantity = FractionHelper.ConvertQuantityBySystemAsync(measurementSystem, i.Quantity, fractionTable),
                    QuantityMax = FractionHelper.ConvertQuantityBySystemAsync(measurementSystem, i.QuantityMax, fractionTable),


                    Unit = MeasurementHelper.BuildUnitDisplayString(MeasurementHelper.GetLargerQuantity(i.Quantity, i.QuantityMax), i.Unit, recipe.ShowAbbreviations, unitTable).ToString(),
                    Description = i.Description,
                    Instructions = i.Instructions,
                    SortOrder = i.SortOrder ?? 0,
                    IsActive = i.IsActive
                })
                .ToList();


            // Map steps to DTOs
            var stepDtos = steps
                .Select(s => new StepDto
                {
                    Id = s.Id,
                    Description = s.Description,
                    SortOrder = s.SortOrder,
                    IsActive = s.IsActive
                })
                .ToList();

            // Build recipe DTO
            var dto = new PlatformAPI.DTO.Recipe.ViewRecipeDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ShowAbbreviations = recipe.ShowAbbreviations,
                IsActive = recipe.IsActive,
                SortOrder = recipe.SortOrder,
                IsMyRecipe = Convert.ToInt32(userId) == recipe.UserRecipe.UserId,
                RecipeVisibility = recipe.RecipeVisibility.ToString(),
                RecipeFont = recipe.RecipeFont.ToString(),
                Ingredients = ingredientDtos,
                Steps = stepDtos,
                MeasurementSystem = measurementSystem.ToString()
            };

            return Ok(dto);
        }

        [HttpGet("isMyRecipe/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> IsMyRecipe(int recipeId)
        {
            // Extract UserId from claims
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
                return Unauthorized("UserId claim missing or invalid.");

            // Query the UserRecipe table to check ownership
            var ownership = await _context.UserRecipes
                .FirstOrDefaultAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);

            // Return a simple boolean
            return Ok(new
            {
                isOwner = ownership != null
            });
        }


        #region Auxilliary Functions

        private async Task<MeasurementSystem> GetMeasurementSystemByUser(int userId)
        {
            var ms = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.MeasurementSystem)
                .FirstOrDefaultAsync();

            // Because MeasurementSystem is an enum (non-nullable),
            // FirstOrDefaultAsync() returns the *default enum value* if no user is found.
            // If your default enum value is not Imperial, enforce it manually:
            if (!Enum.IsDefined(typeof(MeasurementSystem), ms))
                return MeasurementSystem.Imperial;

            return ms;
        }

        #endregion
    }
}
