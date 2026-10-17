namespace PlatformAPI.Models.UnitConversion
{
    public class IngredientUnit
    {
        public int Id { get; set; }

        public string Ingredient { get; set; } = string.Empty;

        public string LargeImperialUnit { get; set; } = string.Empty;

        public string SmallImperialUnit { get; set; } = string.Empty;

        public string MetricUnit { get; set; } = string.Empty;

        public int Pass { get; set; }

        public bool IsLiquid { get; set; }
    }
}
