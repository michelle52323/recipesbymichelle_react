using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using PlatformAPI.Models.Categories;
using PlatformAPI.Enums;

namespace PlatformAPI.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // DTO for incoming request
        public class CreateCategoryDto
        {
            public string Name { get; set; }
        }

        #region Shared Endpoint Functions

        // ---------------------------------------------------------
        // ⭐ SHARED AUX METHODS (REAL + MOCK CALL THESE)
        // ---------------------------------------------------------

        private async Task<IActionResult> AddCategoryInternal(int userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Category name is required.");

            // Get max sort order for this user
            var maxSortOrder = await _context.Categories
                .Where(c => c.UserId == userId && c.IsActive)
                .MaxAsync(c => (int?)c.SortOrder) ?? 0;

            var category = new Category
            {
                UserId = userId,
                Name = name.Trim(),
                SortOrder = maxSortOrder + 1,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                categoryId = category.Id,
                name = category.Name,
                sortOrder = category.SortOrder
            });
        }

        private async Task<IActionResult> GetCategoriesInternal(int userId, SortBy sortBy)
        {
            var query = _context.Categories
                .Where(c => c.UserId == userId && c.IsActive);

            switch (sortBy)
            {
                case SortBy.SortOrder:
                    query = query.OrderBy(c => c.SortOrder);
                    break;

                case SortBy.Alphabetical:
                    query = query.OrderBy(c => c.Name);
                    break;

                default:
                    return BadRequest("Invalid sort option.");
            }

            var categories = await query.ToListAsync();
            return Ok(categories);
        }

        #endregion



        #region Real Endpoints

        // ---------------------------------------------------------
        // ⭐ REAL ENDPOINTS (Authorize)
        // ---------------------------------------------------------

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
        {
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
                return Unauthorized("UserId claim missing or invalid.");

            return await AddCategoryInternal(userId, dto.Name);
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetCategories([FromQuery] SortBy sortBy)
        {
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
                return Unauthorized("UserId claim missing or invalid.");

            return await GetCategoriesInternal(userId, sortBy);
        }

        #endregion

        #region Mock Endpoints

        // ---------------------------------------------------------
        // ⭐ MOCK ENDPOINTS (NO Authorize)
        // ---------------------------------------------------------

        [HttpPost("addMock")]
        public async Task<IActionResult> AddCategoryMock([FromBody] CreateCategoryDto dto)
        {
            const int mockUserId = 10;
            return await AddCategoryInternal(mockUserId, dto.Name);
        }

        [HttpGet("listMock")]
        public async Task<IActionResult> GetCategoriesMock([FromQuery] SortBy sortBy)
        {
            const int mockUserId = 10;
            return await GetCategoriesInternal(mockUserId, sortBy);
        }

        #endregion
    }
}
