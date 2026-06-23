using PlatformAPI.DTO.Recipe;
using PlatformAPI.Models.Recipe;
using System.Text.RegularExpressions;
using PlatformAPI.Enums;
using PlatformAPI.Helpers;
using PlatformAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PlatformAPI.RecipeImport.Parser
{

    public class LineWorkItem
    {
        public string OriginalLine { get; set; }
        public string WorkingLine { get; set; }
        public ConvertRecipeIngredientDto Ingredient { get; set; }
    }

    public class RecipeParser
    {

        private readonly AppDbContext _context;

        public RecipeParser(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConvertRecipeIntermediateDto> ConvertTextToRecipeDto(
        string text,
        MeasurementSystem measurementSystem)
        {
            UnitDB unitDB = new UnitDB(_context);

            // 1. Load valid units once
            var validUnits = await unitDB.LoadUnitTableAsync();

            // 2. Split into lines
            var lines = text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            // Rename to preliminaryIngredientList
            var preliminaryIngredientList = new List<ConvertRecipeIngredientDto>();

            IngredientParser ingredientParser = new IngredientParser();

            // 3. Parse each line as an ingredient (for now)
            foreach (var line in lines)
            {
                string workingLine = line;

                var parsed = ingredientParser.ParseIngredientLine(
                    workingLine,
                    measurementSystem,
                    validUnits
                );

                var ingredientDto = new ConvertRecipeIngredientDto
                {
                    OriginalLine = line,
                    Quantity = parsed.Quantity,
                    Unit = parsed.Unit,
                    Description = parsed.Description,
                    Instructions = parsed.Instructions,
                    IsIngredient = parsed.IsIngredient
                };

                preliminaryIngredientList.Add(ingredientDto);
            }

            // ------------------------------------------------------------
            // NEW LOGIC: Identify ingredient block + topLines + bottomLines
            // ------------------------------------------------------------

            // Find first and last ingredient
            int firstIndex = preliminaryIngredientList.FindIndex(x => x.IsIngredient);
            int lastIndex = preliminaryIngredientList.FindLastIndex(x => x.IsIngredient);

            // If no ingredients found, everything is topLines
            if (firstIndex == -1)
            {
                return new ConvertRecipeIntermediateDto
                {
                    Ingredients = new List<ConvertRecipeIngredientDto>(),
                    Steps = new List<ConvertRecipeStepDto>(),
                    MeasurementSystem = measurementSystem,
                    TopLines = preliminaryIngredientList.Select(x => x.OriginalLine).ToList(),
                    BottomLines = new List<string>()
                };
            }

            // Build ingredientList (copies)
            var ingredientList = preliminaryIngredientList
                .Skip(firstIndex)
                .Take(lastIndex - firstIndex + 1)
                .ToList();

            // Build topLines (OriginalLine only)
            var topLines = preliminaryIngredientList
                .Take(firstIndex)
                .Select(x => x.OriginalLine)
                .ToList();

            // Build bottomLines (OriginalLine only)
            var bottomLines = preliminaryIngredientList
                .Skip(lastIndex + 1)
                .Select(x => x.OriginalLine)
                .ToList();

            // ------------------------------------------------------------
            // Build final DTO
            // ------------------------------------------------------------
            return new ConvertRecipeIntermediateDto
            {
                Ingredients = ingredientList,
                Steps = new List<ConvertRecipeStepDto>(), // still empty
                MeasurementSystem = measurementSystem,
                TopLines = topLines,
                BottomLines = bottomLines
            };
        }

    }
}
