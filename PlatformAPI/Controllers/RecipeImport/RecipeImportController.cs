using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Data;
using PlatformAPI.DTO.Recipe;
using PlatformAPI.Enums;
using PlatformAPI.RecipeImport.Parser;

namespace PlatformAPI.Controllers.RecipeImport
{

    public class IngredientParseRequest
    {
        public string Line { get; set; }
        public string MeasurementSystem { get; set; } // "Imperial" or "Metric"
    }

    public class ConvertRecipeRequest
    {
        public string Text { get; set; }
        public string MeasurementSystem { get; set; }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class RecipeImportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipeImportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("test-ingredient")]
        public async Task<IActionResult> TestIngredientParser([FromBody] IngredientParseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Line))
                return BadRequest("Ingredient line cannot be empty.");

            if (string.IsNullOrWhiteSpace(request.MeasurementSystem))
                return BadRequest("Measurement system is required.");

            // Normalize measurement system
            string system = request.MeasurementSystem.Trim();

            if (!system.Equals("Imperial", StringComparison.OrdinalIgnoreCase) &&
                !system.Equals("Metric", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Measurement system must be 'Imperial' or 'Metric'.");
            }

            MeasurementSystem systemEnum =
                system.Equals("Imperial", StringComparison.OrdinalIgnoreCase)
                    ? MeasurementSystem.Imperial
                    : MeasurementSystem.Metric;


            // Load valid units once
            var validUnits = await _context.Units.ToListAsync();

            // Create parser
            var parser = new IngredientParser();

            // Parse
            var dto = parser.ParseIngredientLine(
                request.Line,
                systemEnum,
                validUnits
            );

            // Validate result
            if (string.IsNullOrWhiteSpace(dto.Description))
                return BadRequest("Unable to parse ingredient description.");

            return Ok(dto);
        }

        [HttpPost("convert")]
        public async Task<ActionResult<ConvertRecipeIntermediateDto>> ConvertRecipe([FromBody] ConvertRecipeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Text cannot be empty.");

            string system = request.MeasurementSystem?.Trim() ?? "";

            if (!system.Equals("Imperial", StringComparison.OrdinalIgnoreCase) &&
                !system.Equals("Metric", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Measurement system must be 'Imperial' or 'Metric'.");
            }

            // Convert string → enum
            MeasurementSystem systemEnum =
                system.Equals("Imperial", StringComparison.OrdinalIgnoreCase)
                    ? MeasurementSystem.Imperial
                    : MeasurementSystem.Metric;

            RecipeParser recipeParser = new RecipeParser(_context);

            // Call your parser
            var dto = await recipeParser.ConvertTextToRecipeDto(request.Text, systemEnum);

            return Ok(dto);
        }

    }
}
