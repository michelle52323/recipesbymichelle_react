using System.ComponentModel.DataAnnotations;

namespace PlatformAPI.Models.Recipe
{
    public class Unit
    {
        public int Id { get; set; } // Primary Key

        [MaxLength(50)]
        public string? Description { get; set; } // e.g., "Tablespoon", "Gram"

        [MaxLength(50)]
        public string? Abbreviation { get; set; } // e.g., "tbsp", "g"

        public int? System { get; set; } // e.g., 1 = Metric, 2 = Imperial

        [MaxLength(50)]
        public string? Plural { get; set; } // e.g., "Tablespoons", "Grams"
    }
}
