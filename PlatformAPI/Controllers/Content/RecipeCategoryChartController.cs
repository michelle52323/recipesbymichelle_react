using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using System.Linq;

namespace PlatformAPI.Controllers.CookingTips
{
    #region DTOs

    public class RecipeCategoryChartItemDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }

    public class RecipeCategoryChartResponseDto
    {
        public List<RecipeCategoryChartItemDto> Categories { get; set; }
        public int UncategorizedCount { get; set; }
    }



    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class RecipeCategoryChartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipeCategoryChartController(AppDbContext context)
        {
            _context = context;
        }

        #region Shared Endpoint Functions

        private async Task<RecipeCategoryChartResponseDto> BuildCategoryChartAsync(int userId)
        {
            // Pull all active categories for this user
            var categories = await _context.Categories
                .Where(c => c.UserId == userId && c.IsActive)
                .ToListAsync();

            // Pull all active recipes for this user
            var recipes = await _context.Recipes
                .Include(r => r.UserRecipe)
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .Where(r => r.UserRecipe.UserId == userId && r.IsActive)
                .ToListAsync();

            // Count uncategorized recipes
            int uncategorizedCount = recipes.Count(r =>
                r.RecipeCategories == null ||
                r.RecipeCategories.Count == 0);

            // Build category counts
            var categoryDtos = categories
                .Select(cat =>
                {
                    int count = recipes.Count(r =>
                        r.RecipeCategories != null &&
                        r.RecipeCategories.Any(rc => rc.CategoryId == cat.Id));

                    return new RecipeCategoryChartItemDto
                    {
                        CategoryId = cat.Id,
                        CategoryName = cat.Name ?? "",
                        Count = count
                    };
                })
                .OrderByDescending(dto => dto.Count)
                .ToList();

            return new RecipeCategoryChartResponseDto
            {
                Categories = categoryDtos,
                UncategorizedCount = uncategorizedCount
            };
        }

        #endregion

        #region Real Endpoints

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetRealCategoryChart()
        {
            // Extract UserId from claims
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
                return Unauthorized("UserId claim missing or invalid.");

            var result = await BuildCategoryChartAsync(userId);
            return Ok(result);
        }

        #endregion

        #region Mock Endpoints

        [HttpGet("getMock")]
        public async Task<IActionResult> GetMockCategoryChart()
        {
            const int mockUserId = 10;

            var result = await BuildCategoryChartAsync(mockUserId);
            return Ok(result);
        }

        #endregion
    }
}
