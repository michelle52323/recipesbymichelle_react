using PlatformAPI.Enums;

namespace PlatformAPI.Models.UnitConversion
{
    public class IngredientUnit
    {
        public int Id { get; set; }

        public string Ingredient { get; set; } = string.Empty;

        public string LargeImperialUnit { get; set; } = string.Empty;

        public string SmallImperialUnit { get; set; } = string.Empty;

        public string LargeMetricUnit {  get; set; } = string.Empty;

        public string MetricUnit { get; set; } = string.Empty;

        public int Pass { get; set; }

        //Items marked as IsLiquid = true must be scanned before non liquid items
        //Example: Cream of mushroom soup.  Soup must be scanned first.  If using
        //cream of mushroom soup in recipe, for unit conversion purposes, this counts as
        //soup, not as mushroom
        public bool IsLiquid { get; set; }

        public IngredientType IngredientType { get; set;}
    }
}
