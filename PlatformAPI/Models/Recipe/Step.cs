namespace PlatformAPI.Models.Recipe
{
    public class Step
    {
        public int Id { get; set; } // Primary Key

        public string? Description { get; set; } // Nullable text

        public int? SortOrder { get; set; } // Nullable int for ordering

        public bool IsActive { get; set; } // Required flag

        public RecipeStep RecipeStep { get; set; }
    }
}
