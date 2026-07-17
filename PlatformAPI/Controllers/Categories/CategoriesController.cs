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
    public class CreateCategoryDto
    {
        public string Name { get; set; }
    }

    public class RenameCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CategorySortOrderDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }


    public class DeleteCategoryDto
    {
        public int Id { get; set; }
    }


    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
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

        private async Task<bool> RenameCategoryInternal(int id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return false;

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            category.Name = newName.Trim();
            await _context.SaveChangesAsync();

            return true;
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

        [Authorize]
        [HttpPost("rename")]
        public async Task<IActionResult> RenameCategory([FromBody] RenameCategoryDto dto)
        {
            var success = await RenameCategoryInternal(dto.Id, dto.Name);

            if (!success)
                return BadRequest(new { success = false });

            return Ok(new { success = true });
        }

        [HttpPost("updateSortOrder")]
        [Authorize]
        public async Task<IActionResult> UpdateSortOrder([FromBody] List<CategorySortOrderDto> updates)
        {
            try
            {
                // Extract UserId from auth claim
                var userId = int.Parse(User.FindFirst("UserId").Value);

                // Extract category IDs from payload
                var categoryIds = updates.Select(c => c.Id).ToList();

                // Load only the user's active categories
                var categories = await _context.Categories
                    .Where(c => categoryIds.Contains(c.Id)
                                && c.IsActive
                                && c.UserId == userId)
                    .ToListAsync();

                // Apply updates
                foreach (var category in categories)
                {
                    var update = updates.FirstOrDefault(u => u.Id == category.Id);
                    if (update != null)
                    {
                        category.SortOrder = update.SortOrder;
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



        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryDto dto)
        {
            // Extract UserId from auth claim
            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
                return Unauthorized(new { success = false, message = "Invalid user claim." });

            // Find category
            var category = await _context.Categories.FindAsync(dto.Id);

            if (category == null)
                return NotFound(new { success = false, message = "Category not found." });

            // Validate ownership
            if (category.UserId != userId)
                return Forbid(); // cannot send an object here

            // Soft delete
            category.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
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

        [HttpPost("renameMock")]
        public async Task<IActionResult> RenameCategoryMock([FromBody] RenameCategoryDto dto)
        {
            const int mockUserId = 10;

            var success = await RenameCategoryInternal(dto.Id, dto.Name);

            if (!success)
                return BadRequest(new { success = false });

            return Ok(new { success = true });
        }


        #endregion
    }
}
