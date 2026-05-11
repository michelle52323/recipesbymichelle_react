using PlatformAPI.Enums;

namespace PlatformAPI.Models.Recipe
{
    public class Recipe
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool ShowAbbreviations { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }

        public RecipeVisibility RecipeVisibility { get; set; }

        public UserRecipe UserRecipe { get; set; }
    }
}
