using Microsoft.AspNetCore.Mvc;
using PlatformAPI.Data;
using PlatformAPI.DTO.Recipe;
using PlatformAPI.Enums;
using PlatformAPI.RecipeImport;
using PlatformAPI.RecipeImport.Parser;
using PlatformAPI.Service;
using System.Text.Json;

namespace PlatformAPI.Controllers.RecipeImport
{

    public class UrlRequest
    {
        public string Url { get; set; }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class UrlScraperController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RecipeScraperService _scraperService;

        public UrlScraperController(AppDbContext context, RecipeScraperService scraperService)
        {
            _context = context;
            _scraperService = scraperService;
        }

        // ------------------------------------------------------------
        // POST: /api/recipes/import/scrapeUrl?url=https://example.com/recipe
        // ------------------------------------------------------------
        [HttpPost("/api/recipes/import/scrapeUrl")]
        public async Task<IActionResult> ScrapeRecipe([FromBody] UrlRequest request)
        {

            int userId = int.TryParse(
                User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                out var parsedId
            ) ? parsedId : 0;

            if (userId == 0)
            {
                return Unauthorized("UserId claim missing or invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Url))
                return BadRequest(new { error = "URL is required." });

            var recipeJson = await _scraperService.ExtractRecipeJsonAsync(request.Url);

            if (recipeJson == null)
            {
                return NotFound(new
                {
                    error = "No recipe metadata found at the provided URL.",
                    request.Url
                });
            }

            RecipeParser recipeParser = new RecipeParser(_context);
            MeasurementSystem measurementSystem = MeasurementSystem.Imperial;

            ConvertRecipeDto recipeReturn = await recipeParser.ConvertJsonLdToRecipeDto(recipeJson, measurementSystem);

            if (recipeReturn != null)
            {
                DatabaseFunctions df = new DatabaseFunctions(_context);
                var result = await df.InsertImportedRecipe(recipeReturn, userId);

                dynamic r = result;

                if (r.success)
                {
                    return Ok(new
                    {
                        status = 200,
                        success = true,
                        request.Url,
                        recipeId = r.recipeId,
                        recipe = recipeReturn
                    });
                }
                else
                {
                    // failure path
                    return BadRequest(new
                    {
                        error = r.message,
                        request.Url
                    });
                    // handle error
                }

            }

            // Return the raw JSON-LD recipe object
            return NotFound(new
            {
                error = "No recipe metadata found at the provided URL.",
                request.Url
            });

        }

        [HttpGet("test")]
        public async Task<IActionResult> ScrapeRecipeTest([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest(new { error = "URL is required." });

            var recipeJson = await _scraperService.ExtractRecipeJsonAsync(url);

            if (recipeJson == null)
            {
                return NotFound(new
                {
                    error = "No recipe metadata found at the provided URL.",
                    url
                });
            }

            RecipeParser recipeParser = new RecipeParser(_context);
            MeasurementSystem measurementSystem = MeasurementSystem.Imperial;
            //JsonDocument doc = JsonDocument.Parse(recipeJson);
            //JsonElement root = doc.RootElement;

            //var recipeReturn = recipeParser.ConvertJsonLdToRecipeDto(recipeJson, measurementSystem);
            ConvertRecipeDto recipeReturn = await recipeParser.ConvertJsonLdToRecipeDto(recipeJson, measurementSystem);


            // Return the raw JSON-LD recipe object
            return Ok(new
            {
                url,
                recipe = recipeReturn
            });
        }
    }
}
