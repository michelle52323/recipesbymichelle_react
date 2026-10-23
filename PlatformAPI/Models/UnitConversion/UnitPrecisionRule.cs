namespace PlatformAPI.Models.UnitConversion
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UnitPrecisionRule
    {
        public int Id { get; set; }

        [Required]
        public string Unit { get; set; }

        public string? Comparison { get; set; }

        public float? Threshold { get; set; }

        public string? NewUnit { get; set; }

        public float? ConversionFactor { get; set; }

        public int? Condition { get; set; }

        public float? RoundToConditional { get; set; }

        public float? RoundToElse { get; set; }
    }

}
