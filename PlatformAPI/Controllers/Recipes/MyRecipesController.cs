using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace PlatformAPI.Controllers.Recipes
{

    #region DTOs
    public class MyRecipesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool ShowAbbreviations { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }

    public class RecipeSortOrderDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }



    #endregion


    [ApiController]
    [Route("api/[controller]")]
    public class MyRecipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MyRecipesController(AppDbContext context)
        {
            _context = context;
        }

        #region Get functions
        [HttpGet("getRecipes")]
        public async Task<IActionResult> GetRecipes()
        {
            // Extract UserId from claims
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
            {
                return Unauthorized("UserId claim missing or invalid.");
            }

            // Query recipes for this user
            var recipeList = await _context.Recipes
                .Include(r => r.UserRecipe)
                .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                .OrderBy(r => r.SortOrder)
                .ToListAsync();

            // Map to DTO
            var dto = recipeList.Select(r => new MyRecipesDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ShowAbbreviations = r.ShowAbbreviations,
                IsActive = r.IsActive,
                SortOrder = r.SortOrder
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("getRecipesMock")]
        public async Task<IActionResult> GetRecipesMock()
        {
            int userId = 10;

            // Query recipes for this user
            var recipeList = await _context.Recipes
                .Include(r => r.UserRecipe)
                .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                .OrderBy(r => r.SortOrder)
                .ToListAsync();

            // Map to DTO
            var dto = recipeList.Select(r => new MyRecipesDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ShowAbbreviations = r.ShowAbbreviations,
                IsActive = r.IsActive,
                SortOrder = r.SortOrder
            }).ToList();

            return Ok(dto);
        }

        #endregion


        #region Update Sort Order

        [HttpPost("updateSortOrder")]
        [Authorize]
        public async Task<IActionResult> UpdateSortOrder([FromBody] List<RecipeSortOrderDto> updates)
        {

            try
            {
                var userId = int.Parse(User.FindFirst("UserId").Value);

                var recipeIds = updates.Select(r => r.Id).ToList();

                var recipes = await _context.Recipes
                    .Where(r => recipeIds.Contains(r.Id) && r.IsActive && r.UserRecipe.UserId == userId)
                    .ToListAsync();

                foreach (var recipe in recipes)
                {
                    var update = updates.FirstOrDefault(u => u.Id == recipe.Id);
                    if (update != null)
                    {
                        recipe.SortOrder = update.SortOrder;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(id);
                if (recipe == null)
                {
                    return NotFound();
                }

                recipe.IsActive = false;
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
