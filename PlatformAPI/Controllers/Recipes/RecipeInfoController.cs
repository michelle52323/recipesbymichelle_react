using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using System.Security.Claims;
using PlatformAPI.Models.Users;
using Microsoft.Data.SqlClient;
using PlatformAPI.Enums;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Models.Categories;

namespace PlatformAPI.Controllers.Recipes
{

    #region DTO

    public class RecipeInfoCategoriesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int SortOrder { get; set; }
    }

    public class SaveRecipeCategoriesDto {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }


    public class RecipeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool ShowAbbreviations { get; set; }

        public string RecipeVisibility { get; set; }

        public string RecipeFont {  get; set; }

        public string UserId { get; set; }

        public List<RecipeInfoCategoriesDto>? Categories { get; set; }
    }

    public class CreateRecipeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ShowAbbreviations { get; set; }
        public string RecipeVisibility { get; set; }

        public string RecipeFont { get; set; }

        public List<SaveRecipeCategoriesDto>? Categories { get; set; }
    }
    public class EditRecipeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool ShowAbbreviations { get; set; }

        public string RecipeVisibility { get; set; }

        public string RecipeFont { get; set; }

        public List<SaveRecipeCategoriesDto>? Categories { get; set; }
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
        public async Task<IActionResult> GetRecipe(int id)
        {
            // Extract UserId from claims
            //int userId = int.TryParse(
            //    User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
            //    out var parsedId
            //) ? parsedId : 0;

            //if (userId == 0)
            //{
            //    return Unauthorized("UserId claim missing or invalid.");
            //}

            // Load recipe + categories
            var recipe = await _context.Recipes
                .Include(r => r.UserRecipe)
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            // Map to DTO
            var dto = new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ShowAbbreviations = recipe.ShowAbbreviations,
                RecipeVisibility = recipe.RecipeVisibility.ToString(),
                RecipeFont = recipe.RecipeFont.ToString(),
                UserId = recipe.UserRecipe.UserId.ToString(),

                Categories = recipe.RecipeCategories?
                    .Select(rc => new RecipeInfoCategoriesDto
                    {
                        Id = rc.CategoryId,
                        Name = rc.Category.Name,
                        SortOrder = rc.SortOrder
                    })
                    .ToList()
            };

            return Ok(dto);
        }


        //[HttpPost("create-recipe")]
        //public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
        //{
        //    try
        //    {
        //        // Get UserId from auth cookie (or claims)
        //        var userId = int.Parse(User.FindFirst("UserId").Value);

        //        var recipe = new Recipe();

        //        // Get max sort order
        //        var maxSortOrder = await _context.Recipes
        //            .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
        //            .MaxAsync(r => (int?)r.SortOrder) ?? 0;

        //        // Populate recipe metadata
        //        recipe.Name = dto.Name;
        //        recipe.Description = dto.Description;
        //        recipe.ShowAbbreviations = dto.ShowAbbreviations;
        //        recipe.RecipeVisibility = Enum.Parse<RecipeVisibility>(dto.RecipeVisibility);
        //        recipe.RecipeFont = Enum.Parse<RecipeFont>(dto.RecipeFont);
        //        recipe.IsActive = true;
        //        recipe.SortOrder = maxSortOrder + 1;

        //        // Add recipe to DB
        //        _context.Recipes.Add(recipe);
        //        await _context.SaveChangesAsync();

        //        // Create UserRecipe link
        //        var userRecipe = new UserRecipe
        //        {
        //            UserId = userId,
        //            RecipeId = recipe.Id
        //        };

        //        _context.UserRecipes.Add(userRecipe);
        //        await _context.SaveChangesAsync();

        //        return Ok(new { success = true, recipeId = recipe.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.ToString() });
        //    }
        //}


        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDto dto)
        {

            // Get UserId from auth cookie (or claims)
            var userId = int.Parse(User.FindFirst("UserId").Value);;

            // Get max sort order
            var maxSortOrder = await _context.Recipes
                .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                .MaxAsync(r => (int?)r.SortOrder) ?? 0;
            try
            {
                // 1️. Create the recipe
                var recipe = new Recipe
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ShowAbbreviations = dto.ShowAbbreviations,
                    RecipeVisibility = Enum.Parse<RecipeVisibility>(dto.RecipeVisibility),
                    RecipeFont = Enum.Parse<RecipeFont>(dto.RecipeFont),
                    IsActive = true,
                    SortOrder = maxSortOrder+1
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();   // ⭐ recipe.Id now exists


                // 2️. Create the UserRecipe link (required for FK chain)
                var userRecipe = new UserRecipe
                {
                    UserId = userId,   // or hardcode for testing
                    RecipeId = recipe.Id
                };

                _context.UserRecipes.Add(userRecipe);
                await _context.SaveChangesAsync();   // ⭐ FK chain now valid


                // 3️⃣ Insert categories (now safe because RecipeId + UserRecipe exist)
                if (dto.Categories != null && dto.Categories.Count > 0)
                {
                    var categoryLinks = dto.Categories.Select(c => new RecipeCategory
                    {
                        RecipeId = recipe.Id,
                        CategoryId = c.Id,
                        SortOrder = c.SortOrder,
                        IsMine = true
                    }).ToList();

                    _context.RecipeCategories.AddRange(categoryLinks);
                    await _context.SaveChangesAsync();
                }

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

                var recipe = await _context.Recipes
                    .Include(r => r.RecipeCategories)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (recipe == null)
                {
                    return NotFound("Recipe not found.");
                }

                // Update basic fields
                recipe.Name = dto.Name;
                recipe.Description = dto.Description;
                recipe.ShowAbbreviations = dto.ShowAbbreviations;
                recipe.RecipeVisibility = Enum.Parse<RecipeVisibility>(dto.RecipeVisibility);
                recipe.RecipeFont = Enum.Parse<RecipeFont>(dto.RecipeFont);

                // ⭐ Clear existing category assignments
                recipe.RecipeCategories.Clear();

                // ⭐ Add new category assignments (if any)
                if (dto.Categories != null)
                {
                    foreach (var c in dto.Categories)
                    {
                        recipe.RecipeCategories.Add(new RecipeCategory
                        {
                            RecipeId = recipe.Id,
                            CategoryId = c.Id,
                            SortOrder = c.SortOrder
                        });
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
