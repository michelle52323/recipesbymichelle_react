using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using PlatformAPI.Models.Categories;
using PlatformAPI.Enums;
using System.Linq;

namespace PlatformAPI.Controllers.Categories
{

    #region DTOs

    public class RecipeCategorySortOrderDto
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
    }


    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class RecipesCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipesCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("updateSortOrder")]
        [Authorize]
        public async Task<IActionResult> UpdateSortOrder([FromBody] List<RecipeCategorySortOrderDto> updates)
        {
            try
            {
                // Extract UserId from auth claim
                var userId = int.Parse(User.FindFirst("UserId").Value);

                // All updates must belong to the same category
                var categoryId = updates.First().CategoryId;

                // Validate category ownership
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId
                                              && c.UserId == userId
                                              && c.IsActive);

                if (category == null)
                    return Forbid(); // user does not own this category

                // Extract recipe IDs
                var recipeIds = updates.Select(u => u.RecipeId).ToList();

                // Validate recipe ownership
                var recipes = await _context.Recipes
                    .Where(r => recipeIds.Contains(r.Id)
                                && r.IsActive)
                    .ToListAsync();

                if (recipes.Count != recipeIds.Count)
                    return Forbid(); // at least one recipe is not owned by the user

                // Load join rows for this category
                var joins = await _context.RecipeCategories
                    .Where(rc => rc.CategoryId == categoryId
                                 && recipeIds.Contains(rc.RecipeId))
                    .ToListAsync();

                if (joins.Count != recipeIds.Count)
                    return BadRequest(new { success = false, message = "One or more recipes are not in this category." });

                // Apply updates
                foreach (var join in joins)
                {
                    var update = updates.FirstOrDefault(u => u.RecipeId == join.RecipeId);
                    if (update != null)
                    {
                        join.SortOrder = update.SortOrder;
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

    }
}
