namespace PlatformAPI.Models.UnitConversion
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ImperialRoundingRule
    {
        public int Id { get; set; }
        public float Lower { get; set; }
        public float Upper { get; set; }
        public string MapToFraction { get; set; }
        public string DisplayOutput { get; set; }
    }

}
