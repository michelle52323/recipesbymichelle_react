using PlatformAPI.DTO.Recipe;
using PlatformAPI.Models.Recipe;
using System.Text.RegularExpressions;
using PlatformAPI.Enums;
using System.Text.Json;

namespace PlatformAPI.RecipeImport.Parser
{
    public class StepParser
    {

        public List<ConvertRecipeStepDto> ExtractStepsFromJsonLd(JsonElement root)
        {
            List<ConvertRecipeStepDto> steps = new();

            if (!root.TryGetProperty("recipeInstructions", out var instArray))
                return steps;

            if (instArray.ValueKind != JsonValueKind.Array)
                return steps;

            int index = 1;

            foreach (var step in instArray.EnumerateArray())
            {
                string? text = null;

                if (step.TryGetProperty("text", out var textProp))
                    text = textProp.GetString();

                if (string.IsNullOrWhiteSpace(text) &&
                    step.TryGetProperty("name", out var nameProp))
                    text = nameProp.GetString();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                steps.Add(new ConvertRecipeStepDto
                {
                    Id = 0,
                    SortOrder = index++,
                    Description = text.Trim(),
                    IsActive = true
                });
            }

            return steps;
        }

    }
}
