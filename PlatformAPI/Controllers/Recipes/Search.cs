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

            if (filtered.Count == 0)
            {
                normalizedTerm = GetSoundExPhrase(searchPhrase);
                filtered = recipeList
                .Where(r =>
                    r.Name != null &&
                    (
                        r.Name.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ||
                        GetSoundExPhrase(r.Name).Contains(normalizedTerm, StringComparison.OrdinalIgnoreCase)
                    )
                )
                .ToList();
            }

            return filtered;
        }


        private string NormalizeWord(string word)
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

        #region SoundEx Algorithm Functions

        private string GetSoundexTerm(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return "0000";

            term = term.Trim().ToUpperInvariant();

            // 1. Preserve first letter
            char firstLetter = term[0];

            // 2. Soundex digit map
            string MapChar(char c) => c switch
            {
                'B' or 'F' or 'P' or 'V' => "1",
                'C' or 'G' or 'J' or 'K' or 'Q' or 'S' or 'X' or 'Z' => "2",
                'D' or 'T' => "3",
                'L' => "4",
                'M' or 'N' => "5",
                'R' => "6",
                _ => "0" // vowels + H, W, Y
            };

            // 3. Map remaining characters to digits
            var encoded = new List<string>();
            string previous = MapChar(firstLetter);

            for (int i = 1; i < term.Length; i++)
            {
                string code = MapChar(term[i]);

                // Skip vowels/H/W/Y but reset duplicate suppression
                if (code == "0")
                {
                    previous = "0";
                    continue;
                }

                // Skip duplicate codes
                if (code == previous)
                    continue;

                encoded.Add(code);
                previous = code;
            }

            // 4. Build final code: first letter + digits
            string result = firstLetter + string.Join("", encoded);

            // 5. Pad or trim to exactly 4 chars
            if (result.Length < 4)
                result = result.PadRight(4, '0');
            else if (result.Length > 4)
                result = result.Substring(0, 4);

            return result;
        }

        private string GetSoundExPhrase(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return string.Empty;

            // Step 1: Normalize the entire phrase (your existing function)
            string normalized = NormalizeWord(phrase);
            // e.g., "mashed potatoes" -> "mash potato"

            // Step 2: Split into individual words
            var tokens = normalized
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Step 3: Convert each token to Soundex
            var soundexTokens = tokens
                .Select(t => GetSoundexTerm(t))
                .ToList();

            // Step 4: Reassemble into a space-separated phrase
            return string.Join(" ", soundexTokens);
        }



        #endregion

        #endregion

    }



}
