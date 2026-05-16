using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using PlatformAPI.Models.Quizzes;
using System.Security.Claims;
using PlatformAPI.Models.Users;
using Microsoft.Data.SqlClient;
using PlatformAPI.Models.StudentQuizzes;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;

namespace PlatformAPI.Controllers.Recipes
{

    #region DTO

    public class RecipeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool ShowAbbreviations { get; set; }

        public string RecipeVisibility { get; set; }

        public string RecipeFont {  get; set; }

        public string UserId { get; set; }
    }

    public class CreateRecipeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ShowAbbreviations { get; set; }
        public string RecipeVisibility { get; set; }

        public string RecipeFont { get; set; }
    }
    public class EditRecipeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool ShowAbbreviations { get; set; }

        public string RecipeVisibility { get; set; }

        public string RecipeFont { get; set; }
    }




    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class RecipeInfoController : ControllerBase
    {

        private readonly AppDbContext _context;

        public RecipeInfoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.UserRecipe)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (recipe == null)
                return NotFound();

            var dto = new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ShowAbbreviations = recipe.ShowAbbreviations,
                RecipeVisibility = recipe.RecipeVisibility.ToString(),
                RecipeFont = recipe.RecipeFont.ToString(),
                UserId = recipe.UserRecipe.UserId.ToString()
            };

            return Ok(dto);
        }

        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
        {
            try
            {
                // Get UserId from auth cookie (or claims)
                var userId = int.Parse(User.FindFirst("UserId").Value);

                var recipe = new Recipe();

                // Get max sort order
                var maxSortOrder = await _context.Recipes
                    .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                    .MaxAsync(r => (int?)r.SortOrder) ?? 0;

                // Populate recipe metadata
                recipe.Name = dto.Name;
                recipe.Description = dto.Description;
                recipe.ShowAbbreviations = dto.ShowAbbreviations;
                recipe.RecipeVisibility = Enum.Parse<RecipeVisibility>(dto.RecipeVisibility);
                recipe.RecipeFont = Enum.Parse<RecipeFont>(dto.RecipeFont);
                recipe.IsActive = true;
                recipe.SortOrder = maxSortOrder + 1;

                // Add recipe to DB
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                // Create UserRecipe link
                var userRecipe = new UserRecipe
                {
                    UserId = userId,
                    RecipeId = recipe.Id
                };

                _context.UserRecipes.Add(userRecipe);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, recipeId = recipe.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.ToString() });
            }
        }

        [HttpPut("update-recipe/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] EditRecipeDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest("Recipe ID mismatch.");
                }

                var recipe = await _context.Recipes.FindAsync(id);
                if (recipe == null)
                {
                    return NotFound("Recipe not found.");
                }

                // Update fields
                recipe.Name = dto.Name;
                recipe.Description = dto.Description;
                recipe.ShowAbbreviations = dto.ShowAbbreviations;
                recipe.RecipeVisibility = Enum.Parse<RecipeVisibility>(dto.RecipeVisibility);
                recipe.RecipeFont = Enum.Parse<RecipeFont>(dto.RecipeFont);

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
