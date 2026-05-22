using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Data;
using Microsoft.AspNetCore.Authorization;
using PlatformAPI.Enums;

namespace PlatformAPI.Controllers.Recipes
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        #region Get Functions

        // ---------------------------------------------------------
        // GET: api/Search/others?query=chicken
        // ---------------------------------------------------------
        [HttpGet("others")]
        [Authorize]
        public async Task<IActionResult> SearchOthers([FromQuery] string? query)
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

            // Base query: only public, active recipes NOT owned by the user
            var recipeList = await _context.Recipes
                .Include(r => r.UserRecipe)
                .Where(r =>
                    r.IsActive &&
                    r.RecipeVisibility == RecipeVisibility.AllUsers &&
                    r.UserRecipe.UserId != userId
                )
                .ToListAsync();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    recipeList = FilterBySearchPhrase(recipeList, query.Trim());
                }


            // Convert to DTO for front-end
            var results = recipeList
                .Select(r => new
                {
                    RecipeId = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsFavorite = _context.Favorites
                        .Any(f => f.UserId == userId && f.RecipeId == r.Id)
                })
                .OrderBy(r => r.Name) // optional, keeps results stable
                .ToList();

            return Ok(results);
        }

        #endregion

        #region Auxilliary Functions

        private List<PlatformAPI.Models.Recipe.Recipe> FilterBySearchPhrase(
        List<PlatformAPI.Models.Recipe.Recipe> recipeList,
        string searchPhrase
        )
        {
            if (recipeList == null)
                return new List<PlatformAPI.Models.Recipe.Recipe>();

            if (string.IsNullOrWhiteSpace(searchPhrase))
                return recipeList;

            var normalizedTerm = NormalizeWord(searchPhrase);

            var filtered = recipeList
                .Where(r =>
                    r.Name != null &&
                    (
                        r.Name.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ||
                        NormalizeWord(r.Name).Contains(normalizedTerm, StringComparison.OrdinalIgnoreCase)
                    )
                )
                .ToList();

            return filtered;
        }


        string NormalizeWord(string word)
        {
            word = word.ToLower();

            var fragileWords = new HashSet<string> { "pies", "fries", "cookies" };

            List<string> tokens = word.Split(' ').ToList();

            var normalizedWord = string.Empty;

            foreach (var token in tokens)
            {
                var (isFragile, newToken) = NormalizeFragileWord(token);

                if (!isFragile)
                {
                    if (token.EndsWith("ies") && token.Length > 3)
                        newToken = token.Substring(0, token.Length - 3) + "y";

                    else if (token.EndsWith("ied") && token.Length > 4)
                        newToken = token.Substring(0, token.Length - 3) + "y";

                    else if (token.EndsWith("es") && token.Length > 2)
                        newToken = token.Substring(0, token.Length - 2);

                    else if (token.EndsWith("ed") && token.Length > 2)
                        newToken = token.Substring(0, token.Length - 2);

                    else if (token.EndsWith("s") && token.Length > 1)
                        newToken = token.Substring(0, token.Length - 1);
                }

                normalizedWord += newToken + " ";
            }

            return normalizedWord.Trim();
        }

        (bool isFragile, string normalized) NormalizeFragileWord(string token)
        {
            var fragileMap = new Dictionary<string, string>
            {
                { "cookies", "cookie" },
                { "fries", "fries" },
                { "pies", "pie" },
                { "veggies", "veggie" },
                { "smoothies", "smoothie" },
                { "goodies", "goodie" },
                { "pickles", "pickle" },
                { "noodles", "noodle" },
                { "sauces", "sauce" },
                { "spices", "spice" },
                { "cakes", "cake" }

            };

            return fragileMap.ContainsKey(token)
                ? (true, fragileMap[token])
                : (false, token);
        }

        #endregion

    }



}
