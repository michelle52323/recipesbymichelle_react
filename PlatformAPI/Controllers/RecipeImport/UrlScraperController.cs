using Microsoft.AspNetCore.Mvc;
using PlatformAPI.Data;
using PlatformAPI.Enums;
using PlatformAPI.RecipeImport.Parser;
using PlatformAPI.Service;
using System.Text.Json;

namespace PlatformAPI.Controllers.RecipeImport
{
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
        // GET: api/UrlScraper?url=https://example.com/recipe
        // ------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> ScrapeRecipe([FromQuery] string url)
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

            var recipeReturn = recipeParser.ConvertJsonLdToRecipeDto(recipeJson, measurementSystem);
            

            // Return the raw JSON-LD recipe object
            return Ok(new
            {
                url,
                recipe = recipeReturn
            });
        }
    }
}
