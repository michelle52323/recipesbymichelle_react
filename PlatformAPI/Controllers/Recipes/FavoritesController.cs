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
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int SortOrder { get; set; }
        public bool IsMine { get; set; }

        public bool IsFavorite { get; set; }

        public object User { get; set; }
        public object Recipe { get; set; }


    }

    #endregion
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFavorites()
        {
            try
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

                // Query all favorites for this user
                var favorites = await _context.Favorites
                    .Include(f => f.Recipe)
                    .Where(f => f.UserId == userId)
                    .OrderBy(f => f.SortOrder)
                    .ToListAsync();

                // Map to DTO list
                var dtoList = favorites.Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    RecipeId = f.RecipeId,
                    SortOrder = f.SortOrder,
                    IsMine = f.IsMine,
                    IsFavorite = true,
                    User = f.User,
                    Recipe = f.Recipe
                }).ToList();

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving favorites: {ex.Message}");
            }
        }


        [HttpGet("{recipeId}")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteByRecipeId(int recipeId)
        {
            try
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

                // Query Favorite table
                var favorite = await _context.Favorites
                    .Include(f => f.Recipe)
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

                if (favorite == null)
                {
                    // Return empty favorite object (your UI expects a consistent shape)
                    return Ok(new FavoriteDto
                    {
                        Id = 0,
                        UserId = userId,
                        RecipeId = recipeId,
                        SortOrder = 0,
                        IsMine = false,
                        IsFavorite = false,
                        User = null,
                        Recipe = null
                    });
                }

                // Map to DTO
                var dto = new FavoriteDto
                {
                    Id = favorite.Id,
                    UserId = favorite.UserId,
                    RecipeId = favorite.RecipeId,
                    SortOrder = favorite.SortOrder,
                    IsMine = favorite.IsMine,
                    IsFavorite = true,
                    User = favorite.User,
                    Recipe = favorite.Recipe
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving favorite: {ex.Message}");
            }
        }

        [HttpPost("{recipeId}")]
        [Authorize]
        public async Task<IActionResult> AddFavorite(int recipeId)
        {
            try
            {
                // Extract UserId
                int userId = int.TryParse(
                    User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                    out var parsedId
                ) ? parsedId : 0;

                if (userId == 0)
                    return Unauthorized("UserId claim missing or invalid.");

                // Check if already exists
                var existing = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

                if (existing != null)
                {
                    // Already exists → return it
                    return Ok(existing);
                }

                // Determine IsMine
                bool isMine = await _context.UserRecipes
                    .AnyAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);

                // Determine SortOrder
                int maxSort = await _context.Favorites
                    .Where(f => f.UserId == userId)
                    .Select(f => (int?)f.SortOrder)
                    .MaxAsync() ?? 0;

                int newSortOrder = maxSort + 1;

                // Create new favorite
                var favorite = new Favorite
                {
                    UserId = userId,
                    RecipeId = recipeId,
                    SortOrder = newSortOrder,
                    IsMine = isMine
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                // Load nav properties
                await _context.Entry(favorite).Reference(f => f.User).LoadAsync();
                await _context.Entry(favorite).Reference(f => f.Recipe).LoadAsync();

                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding favorite: {ex.Message}");
            }
        }



        [HttpDelete("{recipeId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(int recipeId)
        {
            try
            {
                // Extract UserId
                int userId = int.TryParse(
                    User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                    out var parsedId
                ) ? parsedId : 0;

                if (userId == 0)
                    return Unauthorized("UserId claim missing or invalid.");

                // Find favorite
                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error removing favorite: {ex.Message}");
            }
        }


    }
}
