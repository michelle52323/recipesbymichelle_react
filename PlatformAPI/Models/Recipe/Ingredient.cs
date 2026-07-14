using System.ComponentModel.DataAnnotations;

namespace PlatformAPI.Models.Recipe
{
    public class Ingredient
    {
        public int Id { get; set; } // Primary Key

        public float? Quantity { get; set; } // Optional quantity value

        public float? QuantityMax { get; set; } // Optional quantity max value, handles ranges

        [StringLength(50)]
        public string? Unit { get; set; } // e.g., "cup", "tbsp"

        [StringLength(255)]
        public string? Description { get; set; } // e.g., "chopped onions"

        [StringLength(255)]
        public string? Instructions { get; set; } // e.g., "sauté until golden"

        public int? SortOrder { get; set; } // Optional display order

        [Required]
        public bool IsActive { get; set; } // Indicates if the item is in use

        public RecipeIngredient RecipeIngredient { get; set; }
    }
}
