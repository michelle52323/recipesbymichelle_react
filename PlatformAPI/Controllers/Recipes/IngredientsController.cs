using Microsoft.AspNetCore.Mvc;
using PlatformAPI.Data;
using PlatformAPI.Helpers;
using PlatformAPI.DTO.Recipe;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;

namespace PlatformAPI.Controllers.Recipes
{
    #region DTOs


    public class IngredientGridDto
    {
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<UnitDto> ValidUnits { get; set; } = new();
    }

    #endregion


    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ingredients/{recipeId}")]
        public async Task<ActionResult<List<IngredientDto>>> GetIngredients(int recipeId)
        {
            // Load the recipe to ensure it exists
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == recipeId && r.IsActive);

            if (recipe == null)
                return NotFound();

            // Load ingredients for this recipe
            var ingredients = await _context.Ingredients
                .Include(i => i.RecipeIngredient)
                .Where(i => i.RecipeIngredient.RecipeId == recipeId && i.IsActive)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            // Load unit lookup table (if needed for display)
            var unitDB = new UnitDB(_context);
            List<Unit> unitTable = await unitDB.LoadUnitTableAsync();

            // Determine measurement system from user claims
            var claimValue = User?
                .Claims
                .FirstOrDefault(c => c.Type == "MeasurementSystem")
                ?.Value;

            MeasurementSystem measurementSystem =
                Enum.TryParse<MeasurementSystem>(claimValue, ignoreCase: true, out var parsedEnum)
                ? parsedEnum
                : MeasurementSystem.Imperial;

            // Load fraction table if needed
            List<FractionDecimal>? fractionTable = null;

            if (measurementSystem == MeasurementSystem.Imperial)
            {
                var fractionDb = new FractionHelperDB(_context);
                fractionTable = await fractionDb.LoadFractionTableAsync();
            }

            // Map to DTOs
            var ingredientDtos = ingredients
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Quantity = FractionHelper.ConvertQuantityBySystemAsync(
                        measurementSystem,
                        i.Quantity,
                        fractionTable
                    ),
                    Unit = MeasurementHelper.BuildUnitDisplayString(
                        i.Quantity,
                        i.Unit,
                        recipe.ShowAbbreviations,
                        unitTable
                    ).ToString(),
                    Description = i.Description,
                    Instructions = i.Instructions,
                    SortOrder = i.SortOrder ?? 0,
                    IsActive = i.IsActive
                })
                .ToList();

            return Ok(ingredientDtos);
        }

    }
}
