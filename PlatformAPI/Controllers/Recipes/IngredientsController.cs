using Microsoft.AspNetCore.Mvc;
using PlatformAPI.Data;
using PlatformAPI.Helpers;
using PlatformAPI.DTO.Recipe;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;
using Microsoft.AspNetCore.Authorization;

namespace PlatformAPI.Controllers.Recipes
{
    #region DTOs


    public class IngredientGridDto
    {
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<UnitDto> ValidUnits { get; set; } = new();
    }

    public class InsertIngredientDTO : IngredientDto
    {
        public int RecipeId { get; set; }
    }

    public class IngredientSortOrderDto
    {
        public int Id { get; set; }          // IngredientId
        public int SortOrder { get; set; }
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
                    QuantityMax = FractionHelper.ConvertQuantityBySystemAsync(
                        measurementSystem,
                        i.QuantityMax,
                        fractionTable
                    ),
                    //Unit = MeasurementHelper.BuildUnitDisplayString(
                    //    i.Quantity,
                    //    i.Unit,
                    //    recipe.ShowAbbreviations,
                    //    unitTable
                    //).ToString(),
                    Unit = i.Unit,
                    Description = i.Description,
                    Instructions = i.Instructions,
                    SortOrder = i.SortOrder ?? 0,
                    IsActive = i.IsActive
                })
                .ToList();

            return Ok(ingredientDtos);
        }

        #region Insert / Update functions

        [HttpPost]
        public async Task<IActionResult> InsertIngredient([FromBody] InsertIngredientDTO dto)
        {
            try
            {
                //// 1. Get UserId from claims
                //var userIdClaim = User?
                //    .Claims
                //    .FirstOrDefault(c => c.Type == "User")
                //    ?.Value;


                //if (string.IsNullOrEmpty(userIdClaim))
                //{
                //    return BadRequest(new { success = false, message = "User not authenticated" });
                //}

                //int userId = int.Parse(userIdClaim);

                // 2. Validate RecipeId
                if (dto.RecipeId <= 0)
                {
                    return BadRequest(new { success = false, message = "RecipeId is required" });
                }

                int recipeId = dto.RecipeId;

                // 3. Compute next SortOrder (simplified)
                var maxSortOrder = await _context.Ingredients
                    .Where(i => i.RecipeIngredient.RecipeId == recipeId && i.IsActive)
                    .MaxAsync(i => (int?)i.SortOrder) ?? 0;


                int nextSortOrder = maxSortOrder + 1;

                // 4. Create Ingredient entity
                var ingredient = new Ingredient
                {
                    Quantity = await ConvertQtyToDbFormat(dto.Quantity),
                    QuantityMax = await ConvertQtyToDbFormat(dto.QuantityMax),
                    Unit = dto.Unit,
                    Description = dto.Description,
                    Instructions = dto.Instructions,
                    SortOrder = nextSortOrder,
                    IsActive = true
                };

                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync(); // generates Ingredient.Id

                // 5. Create UserRecipe join record
                var recipeIngredient = new RecipeIngredient
                {
                    RecipeId = recipeId,
                    IngredientId = ingredient.Id
                };

                _context.RecipeIngredients.Add(recipeIngredient);
                await _context.SaveChangesAsync();

                // 6. Return success + updated DTO
                dto.Id = ingredient.Id;
                dto.SortOrder = nextSortOrder;
                dto.IsActive = true;

                return Ok(new { success = true, ingredient = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }



        [HttpPut]
        public async Task<IActionResult> UpdateIngredient([FromBody] IngredientDto dto)
        {
            var ingredient = await _context.Ingredients.FindAsync(dto.Id);
            if (ingredient == null)
                return NotFound();

            // Map DTO → Entity
            ingredient.Quantity = await ConvertQtyToDbFormat(dto.Quantity);
            ingredient.QuantityMax = await ConvertQtyToDbFormat(dto.QuantityMax);
            ingredient.Unit = dto.Unit;
            ingredient.Description = dto.Description;
            ingredient.Instructions = dto.Instructions;
            ingredient.SortOrder = dto.SortOrder ?? ingredient.SortOrder;
            ingredient.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        #endregion

        #region Sorting functions

        [HttpPost("update-sort-order")]
        public async Task<IActionResult> UpdateIngredientSortOrder(

        [FromBody] List<IngredientSortOrderDto> updates)
        {
            try
            {
                // Extract ingredient IDs from payload
                var ingredientIds = updates.Select(u => u.Id).ToList();

                // Load ingredients that belong to this recipe through RecipeIngredient
                var ingredients = await _context.Ingredients
                    .Where(i => ingredientIds.Contains(i.Id)
                                && i.IsActive
                                && i.RecipeIngredient.RecipeId != null)
                    .ToListAsync();

                // Apply updates
                foreach (var ingredient in ingredients)
                {
                    var update = updates.FirstOrDefault(u => u.Id == ingredient.Id);
                    if (update != null)
                    {
                        ingredient.SortOrder = update.SortOrder;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }



        #endregion

        #region Delete functions

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            try
            {
                var ingredient = await _context.Ingredients.FindAsync(id);
                if (ingredient == null)
                {
                    return BadRequest(new { success = false, message = "Ingredient not found" });
                }

                ingredient.IsActive = false;
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }


        #endregion

        #region Auxilliary functions

        private async Task<float?> ConvertQtyToDbFormat(string qtyString)

        {
            FractionHelperDB fh = new FractionHelperDB(_context);

            List<FractionDecimal> FractionDecimalLookupTable = new List<FractionDecimal>();

            FractionDecimalLookupTable = await fh.LoadFractionTableAsync();

            // Determine measurement system from user claims
            var claimValue = User?
                .Claims
                .FirstOrDefault(c => c.Type == "MeasurementSystem")
                ?.Value;

            MeasurementSystem measurementSystem =
                Enum.TryParse<MeasurementSystem>(claimValue, ignoreCase: true, out var parsedEnum)
                ? parsedEnum
                : MeasurementSystem.Imperial;

            float? qtyToInsert = null;

            if (string.IsNullOrWhiteSpace(qtyString))
            {
                qtyToInsert = null;
            }
            else if (measurementSystem == Enums.MeasurementSystem.Imperial)
            {
                qtyToInsert = FractionHelper.ConvertStringToDbValue(qtyString, FractionDecimalLookupTable);
            }
            else if (measurementSystem == Enums.MeasurementSystem.Metric)
            {
                qtyToInsert = float.TryParse(qtyString, out var parsedQty) ? parsedQty : null;
            }

            return qtyToInsert;
        }

        #endregion


    }
}
