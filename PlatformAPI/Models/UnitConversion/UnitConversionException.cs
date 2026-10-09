namespace PlatformAPI.Models.UnitConversion
{
    public class UnitConversionException
    {
        public int Id { get; set; }

        public string Ingredient { get; set; } = string.Empty;

        public int BaseSystem { get; set; }

        public string BaseUnit { get; set; } = string.Empty;

        public int TargetSystem { get; set; }

        public string TargetUnit { get; set; } = string.Empty;

        public float Quantity { get; set; }
    }
}
