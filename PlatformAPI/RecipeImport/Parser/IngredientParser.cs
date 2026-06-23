using PlatformAPI.DTO.Recipe;
using PlatformAPI.Models.Recipe;
using System.Text.RegularExpressions;
using PlatformAPI.Enums;

namespace PlatformAPI.RecipeImport.Parser
{
    public class IngredientParser
    {
        public ConvertRecipeIngredientDto ParseIngredientLine(
            string line,
            MeasurementSystem measurementSystem,
            List<Unit> validUnits
        )
        {
            if (string.IsNullOrWhiteSpace(line))
                return EmptyIngredient();

            // 1. Clean and normalize
            line = CleanLine(line);

            string originalLine = line;

            // 2. Extract instructions (inside parentheses)
            string? instructions = ExtractInstructions(ref line);

            // 3. Extract quantity
            string? quantity = ExtractQuantity(ref line, measurementSystem);

            // 4. Extract unit
            string? unit = ExtractUnit(ref line, validUnits);

            // 5. Remaining text = description
            string description = ExtractDescription(line);

            bool isIngredient = description != null && (unit != null || quantity != null);

            return new ConvertRecipeIngredientDto
            {
                Id = 0,
                Quantity = quantity,
                Unit = unit,
                Description = description,
                Instructions = instructions,
                IsActive = true,
                IsIngredient = isIngredient,
                OriginalLine = originalLine
            };
        }

        private ConvertRecipeIngredientDto EmptyIngredient()
        {
            return new ConvertRecipeIngredientDto
            {
                Id = 0,
                Quantity = null,
                Unit = null,
                Description = "",
                Instructions = null,
                IsActive = true,
                OriginalLine = null
            };
        }

        // ------------------------------------------------------------
        // CLEAN LINE
        // ------------------------------------------------------------
        private string CleanLine(string line)
        {
            line = line.Trim();

            // Remove bullet symbols
            if (line.StartsWith("- ") || line.StartsWith("• ") || line.StartsWith("* "))
                line = line.Substring(2);

            // Normalize multiple spaces
            line = Regex.Replace(line, @"\s+", " ");

            // Normalize unicode fractions (½ → 1/2)
            line = line
                .Replace("½", "1/2")
                .Replace("⅓", "1/3")
                .Replace("⅔", "2/3")
                .Replace("¼", "1/4")
                .Replace("¾", "3/4")
                .Replace("⅛", "1/8")
                .Replace("⅜", "3/8")
                .Replace("⅝", "5/8")
                .Replace("⅞", "7/8");


            return line.Trim();
        }

        // ------------------------------------------------------------
        // INSTRUCTIONS
        // ------------------------------------------------------------
        private string? ExtractInstructions(ref string line)
        {
            // 1. Parentheses take priority
            var parenMatch = Regex.Match(line, @"\((.*?)\)");
            if (parenMatch.Success)
            {
                string instructions = parenMatch.Groups[1].Value.Trim();
                line = Regex.Replace(line, @"\((.*?)\)", "").Trim();
                return instructions;
            }

            // 2. Other separators: comma, semicolon, dash, en-dash, em-dash
            // We look for the FIRST occurrence of any separator
            var separators = new[] { ",", ";", "-", "\u2013","\u2014" };

            int index = -1;
            string? foundSeparator = null;

            foreach (var sep in separators)
            {
                int pos = line.IndexOf(sep);
                if (pos > 0) // must not be at start
                {
                    if (index == -1 || pos < index)
                    {
                        index = pos;
                        foundSeparator = sep;
                    }
                }
            }

            // No separator found → no instructions
            if (index == -1 || foundSeparator == null)
                return null;

            // Extract instructions (everything after the separator)
            string instructionsPart = line.Substring(index + foundSeparator.Length).Trim();

            // Remove from main line
            line = line.Substring(0, index).Trim();

            return string.IsNullOrWhiteSpace(instructionsPart) ? null : instructionsPart;
        }


        // ------------------------------------------------------------
        // QUANTITY
        // ------------------------------------------------------------
        private string? ExtractQuantity(ref string line, MeasurementSystem measurementSystem)
        {
            // IMPERIAL ORDER:
            // 1. Mixed fraction: 1 1/2, 1   1 / 2, etc.
            // 2. Simple fraction: 1/2, 1 / 2
            // 3. Whole number: 1, 12

            // METRIC ORDER:
            // 1. Decimal
            // 2. Whole number

            string? quantity = null;

            if (measurementSystem == MeasurementSystem.Imperial)
            {
                // Mixed fraction
                var mixed = Regex.Match(line, @"^(\d+)\s+(\d+)\s*/\s*(\d+)");
                if (mixed.Success)
                {
                    quantity = $"{mixed.Groups[1].Value} {mixed.Groups[2].Value}/{mixed.Groups[3].Value}";
                    line = line.Substring(mixed.Length).Trim();
                    return quantity;
                }

                // Simple fraction
                var fraction = Regex.Match(line, @"^(\d+)\s*/\s*(\d+)");
                if (fraction.Success)
                {
                    quantity = $"{fraction.Groups[1].Value}/{fraction.Groups[2].Value}";
                    line = line.Substring(fraction.Length).Trim();
                    return quantity;
                }

                // Decimal
                //var dec = Regex.Match(line, @"^(\d*\.\d+)");
                //if (dec.Success)
                //{
                //    quantity = dec.Groups[1].Value;
                //    line = line.Substring(dec.Length).Trim();
                //    return quantity;
                //}

                // Whole number
                var whole = Regex.Match(line, @"^(\d+)");
                if (whole.Success)
                {
                    quantity = whole.Groups[1].Value;
                    line = line.Substring(whole.Length).Trim();
                    return quantity;
                }
            }
            else // METRIC
            {
                // Decimal
                var dec = Regex.Match(line, @"^(\d*\.\d+)");
                if (dec.Success)
                {
                    quantity = dec.Groups[1].Value;
                    line = line.Substring(dec.Length).Trim();
                    return quantity;
                }

                // Whole number
                var whole = Regex.Match(line, @"^(\d+)");
                if (whole.Success)
                {
                    quantity = whole.Groups[1].Value;
                    line = line.Substring(whole.Length).Trim();
                    return quantity;
                }
            }

            return null;
        }

        // ------------------------------------------------------------
        // UNIT
        // ------------------------------------------------------------
        private string? ExtractUnit(ref string line, List<Unit> validUnits)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            string[] parts = line.Split(' ', 2);
            string firstToken = parts[0].Trim();

            // Try matching description, abbreviation, plural
            var match = validUnits.FirstOrDefault(u =>
                string.Equals(u.Description, firstToken, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, firstToken, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, firstToken, StringComparison.OrdinalIgnoreCase)
            );

            if (match == null)
                return null;

            // Remove the unit token from the line
            line = (parts.Length > 1 ? parts[1] : "").Trim();

            return match.Abbreviation ?? match.Description ?? match.Plural;
        }

        // ------------------------------------------------------------
        // DESCRIPTION
        // ------------------------------------------------------------
        private string ExtractDescription(string line)
        {
            return line.Trim();
        }
    }
}
