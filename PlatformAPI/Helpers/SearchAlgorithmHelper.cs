using PlatformAPI.Models.Recipe;

namespace PlatformAPI.Helpers
{
    public class SearchAlgorithmHelper
    {

        public static List<PlatformAPI.Models.Recipe.Recipe> FilterBySearchPhrase(
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

        

        
        public static string NormalizeWord(string word)
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

        public static string NormalizeWordSingular(string word)
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
                    if (token.EndsWith("beans"))
                        newToken = token;

                    else if (token.EndsWith("oats"))
                        newToken = token;

                    else if (token.EndsWith("ies") && token.Length > 3)
                        newToken = token.Substring(0, token.Length - 3) + "y";

                    else if (token.EndsWith("es") && token.Length > 2)
                        newToken = token.Substring(0, token.Length - 2);

                    else if (token.EndsWith("s") && token.Length > 1)
                        newToken = token.Substring(0, token.Length - 1);
                }

                normalizedWord += newToken + " ";
            }

            return normalizedWord.Trim();
        }

        public static string NormalizePhraseSingular(string phrase)
        {
            var words = phrase.Split(' ');
            var newWord = string.Empty;

            foreach (var word in words)
            {
                var normalizedWord = NormalizeWordSingular(word);
                newWord = newWord + ' ' + normalizedWord;
            }

            return newWord;
        }

        public static(bool isFragile, string normalized) NormalizeFragileWord(string token)
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

        #region Unit Table Functions

        public static string GetUnitFullName(List<Unit> unitTable, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return unit;

            var match = unitTable.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase));

            return match?.Description ?? unit;
        }

        public static string GetUnitAbbreviation(List<Unit> unitTable, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return unit;

            var match = unitTable.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                return unit;

            return string.IsNullOrWhiteSpace(match.Abbreviation)
                ? match.Description ?? unit
                : match.Abbreviation;
        }
        #endregion

        #region SoundEx Algorithm Functions

        public static string GetSoundexTerm(string term)
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

        public static string GetSoundExPhrase(string phrase)
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
    }
}
