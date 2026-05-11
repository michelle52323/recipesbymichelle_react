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

            // Find matching unit (by Description or Abbreviation)
            var match = validUnits.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                return unit; // fallback if not found

            // Abbreviation path
            if (showAbbreviations && !string.IsNullOrWhiteSpace(match.Abbreviation))
                return match.Abbreviation;

            // Plural vs singular
            bool isPlural = qty > 1f;

            if (isPlural)
                return match.Plural ?? match.Description ?? unit;

            return match.Description ?? unit;
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
