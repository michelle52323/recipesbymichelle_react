using System.Text.Json;
using HtmlAgilityPack;

namespace PlatformAPI.Service
{
    public class RecipeScraperService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeScraperService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ------------------------------------------------------------
        // PUBLIC ENTRY POINT
        // ------------------------------------------------------------
        public async Task<string?> ExtractRecipeJsonAsync(string url)
        {
            var html = await FetchHtmlAsync(url);
            if (html == null)
                return null;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var scriptNodes = doc.DocumentNode
                .SelectNodes("//script[@type='application/ld+json']");

            if (scriptNodes == null || scriptNodes.Count == 0)
                return null;

            foreach (var node in scriptNodes)
            {
                var rawJson = node.InnerText.Trim();

                JsonElement element;
                try
                {
                    element = JsonSerializer.Deserialize<JsonElement>(rawJson);
                }
                catch
                {
                    continue; // skip invalid JSON blocks
                }

                foreach (var item in NormalizeJsonLd(element))
                {
                    if (item.TryGetProperty("@type", out var typeProp))
                    {
                        var typeValue = typeProp.ToString();

                        if (typeValue.Contains("Recipe", StringComparison.OrdinalIgnoreCase))
                        {
                            // Return the exact JSON for the recipe object
                            return item.GetRawText();
                        }
                    }
                }
            }

            return null; // no recipe found
        }

        // ------------------------------------------------------------
        // FETCH HTML
        // ------------------------------------------------------------
        private async Task<string?> FetchHtmlAsync(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                client.DefaultRequestHeaders.Add("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                return await client.GetStringAsync(url);
            }
            catch
            {
                return null;
            }
        }

        // ------------------------------------------------------------
        // NORMALIZE JSON-LD
        // Handles:
        // - Single object
        // - Array of objects
        // - @graph array
        // ------------------------------------------------------------
        private IEnumerable<JsonElement> NormalizeJsonLd(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                    yield return item;

                yield break;
            }

            if (element.TryGetProperty("@graph", out var graph))
            {
                foreach (var item in graph.EnumerateArray())
                    yield return item;

                yield break;
            }

            yield return element;
        }
    }
}
