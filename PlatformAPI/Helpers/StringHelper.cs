using System.Text.RegularExpressions;

namespace PlatformAPI.Helpers
{
    public class StringHelper
    {

        // ------------------------------------------------------------
        // NORMALIZE UNICODE CHARACTERS
        // ------------------------------------------------------------
        public static string NormalizeUnicodeCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            // Normalize HTML entities
            text = text.Replace("&amp;", "&")
                       .Replace("&amp", "&");

            // Normalize Unicode characters (ampersand and plus)
            // U+0026 = &
            // U+002B = +
            // These are already correct ASCII, but we normalize anyway
            text = text.Replace("\u0026", "&")
                       .Replace("\u002B", "+");

            return text;
        }

        // ------------------------------------------------------------
        // MIXED FRACTION DETECTION — RETURNS STRING "1 1/4"
        // ------------------------------------------------------------
        public static string NormalizeMixedNumberString(string text)
        {
            var mixed = Regex.Match(
                text,
                @"(\d+)\s*(?:[&+]\s*|\s+)(\d+)\s*/\s*(\d+)"
            );

            if (mixed.Success)
            {
                var normalized = $"{mixed.Groups[1].Value} {mixed.Groups[2].Value}/{mixed.Groups[3].Value}";

                // Replace only the matched portion, not the whole string
                text = text.Replace(mixed.Value, normalized);

                return text;
            }

            return text;
        }


        public static string NormalizeUnicodeFractions(string text)
        {
            text = text
                .Replace("½", "1/2")
                .Replace("⅓", "1/3")
                .Replace("⅔", "2/3")
                .Replace("¼", "1/4")
                .Replace("¾", "3/4")
                .Replace("⅛", "1/8")
                .Replace("⅜", "3/8")
                .Replace("⅝", "5/8")
                .Replace("⅞", "7/8");

            return text;
        }


    }
}
