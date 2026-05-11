using System.ComponentModel.DataAnnotations;

namespace PlatformAPI.Models.Recipe
{
    public class FractionDecimal
    {
        public int Id { get; set; } // Primary Key

        [MaxLength(10)]
        public string? Fraction { get; set; } // e.g., "1/2", "3/4"

        public float? Decimal { get; set; } // e.g., 0.5, 0.75

        public bool Primary { get; set; } // Indicates preferred representation
    }
}
