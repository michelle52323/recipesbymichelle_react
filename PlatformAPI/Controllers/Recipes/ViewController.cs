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
            var claimValue = User?
            .Claims
            .FirstOrDefault(c => c.Type == "MeasurementSystem")
            ?.Value;

            MeasurementSystem measurementSystem =
            Enum.TryParse<MeasurementSystem>(claimValue, ignoreCase: true, out var parsedEnum)
            ? parsedEnum
            : MeasurementSystem.Imperial;


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


            // Load the recipe itself
            var recipe = await _context.Recipes
                .Include(r => r.UserRecipe)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (recipe == null)
                return NotFound();

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
                    Unit = MeasurementHelper.BuildUnitDisplayString(i.Quantity, i.Unit, recipe.ShowAbbreviations, unitTable).ToString(),
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
                RecipeVisibility = recipe.RecipeVisibility.ToString(),
                RecipeFont = recipe.RecipeFont.ToString(),
                Ingredients = ingredientDtos,
                Steps = stepDtos,
                MeasurementSystem = measurementSystem
            };

            return Ok(dto);
        }


    }
}
