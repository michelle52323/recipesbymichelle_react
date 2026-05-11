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
        public RecipeVisibility RecipeVisibility { get; set; }

        public List<IngredientDto>? Ingredients { get; set; }
        public List<StepDto>? Steps { get; set; }
    }

    public class ViewRecipeDTO : RecipeDto
    {
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



}
