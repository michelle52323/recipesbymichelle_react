namespace PlatformAPI.Models.UnitConversion
{
    public class UnitConversion
    {
        public int Id { get; set; }

        public int BaseSystem { get; set; }

        public string BaseUnit { get; set; } = string.Empty;

        public int TargetSystem { get; set; }

        public string TargetUnit { get; set; } = string.Empty;

        public float Quantity { get; set; }
    }
}
