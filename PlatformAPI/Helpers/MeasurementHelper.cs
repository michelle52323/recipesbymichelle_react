using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using PlatformAPI.Models.Recipe;
//using PlatformAPI.Models.ViewModels.Recipe;

namespace PlatformAPI.Helpers
{

    public class UnitDB
    {

        private readonly DbContext _context;

        public UnitDB(DbContext context)
        {
            _context = context;
        }

        public async Task<List<Unit>> LoadUnitTableAsync()
        {
            return await _context.Set<Unit>().ToListAsync();
        }

    }
    public class MeasurementHelper
    {

        public static string BuildUnitDisplayString(
        float? qty,
        string? unit,
        bool showAbbreviations,
        List<Unit> validUnits)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return string.Empty;

            var match = validUnits.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                return unit;

            bool isPlural = qty > 1f;

            // First determine the correct full/plural form
            string fullForm = isPlural
                ? (match.Plural ?? match.Description ?? unit)
                : (match.Description ?? unit);

            // THEN override with abbreviation if requested
            if (showAbbreviations && !string.IsNullOrWhiteSpace(match.Abbreviation))
                return match.Abbreviation;

            return fullForm;
        }

        public static float GetLargerQuantity(float? q1, float? q2)
        {
            // Start with the first quantity (it always exists in your model)
            float result = q1 ?? 0f;

            // If q2 exists, compare and take the larger
            if (q2.HasValue)
            {
                result = Math.Max(result, q2.Value);
            }

            return result;
        }









        //public static IHtmlContent BuildUnitDisplayString(string quantity, string? unit, List<Unit> ValidUnits)
        //{
        //    var builder = new HtmlContentBuilder();

        //    if (!string.IsNullOrWhiteSpace(unit))
        //    {
        //        var matchedUnit = ValidUnits
        //            .FirstOrDefault(u =>
        //                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
        //                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase) ||
        //                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase));

        //        var unitDisplay = matchedUnit != null && !string.IsNullOrWhiteSpace(matchedUnit.Abbreviation)
        //            ? matchedUnit.Abbreviation
        //            : unit;

        //        builder.AppendHtml($"{unitDisplay}");
        //    }

        //    return builder;
        //}
    }
}
