using PlatformAPI.Enums;

namespace PlatformAPI.DTO.Recipe
{

    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ShowAbbreviations { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string RecipeVisibility { get; set; }
        public string RecipeFont { get; set; }
    }

    public class ViewRecipeDTO : RecipeDto
    {
        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }
        public MeasurementSystem MeasurementSystem { get; set; }
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class StepDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class UnitDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string? Plural { get; set; }
        public int? System { get; set; }
    }

}
