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

        public bool IsMyRecipe { get; set; }
        public string RecipeVisibility { get; set; }
        public string RecipeFont { get; set; }
    }

    public class ViewRecipeDTO : RecipeDto
    {
        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }
        public string MeasurementSystem { get; set; }

        public List<string> CategoryNames { get; set; }
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string? Quantity { get; set; }
        public string? QuantityMax { get; set; }
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

    public class ConvertRecipeIngredientDto : IngredientDto
    {
        public string OriginalLine { get; set; }
        public bool IsIngredient {  get; set; }
    }

    public class ConvertRecipeStepDto : StepDto
    {

    }


    public class ConvertRecipeDto : RecipeDto
    {
        public List<ConvertRecipeIngredientDto> Ingredients { get; set; }
        public List<ConvertRecipeStepDto> Steps { get; set; }
        public MeasurementSystem MeasurementSystem { get; set; }
    }

    public class ConvertRecipeIntermediateDto : ConvertRecipeDto
    {
        public List<string> TopLines { get; set; }
        public List<string> BottomLines {  get; set; }
    }

    public class IngredientUnitListTestDTO
    {
        public string Ingredient { get; set; }

        public string NormalizedIngredient { get; set; }

        public string BaseUnit { get; set; }

        public string TargetUnit { get; set; }

        public string TargetUnitAlternative { get; set; }

        public float? Quantity { get; set; }

        public float? QuantityMax { get; set; }

        public MeasurementSystem TargetSystem { get; set; }

        public float ConversionFactor { get; set; }

        public float? TargetQuantity { get; set; }

        public float? TargetQuantityMax { get; set; }

        public string? FinalQuantity {  get; set; }

        public string? FinalQuantityMax { get; set; }

        public int? SortOrder { get; set; }

        public bool Convert { get; set; }

        public bool IsActive { get; set; }
    }

    public class IngredientConversionTestList
    {
        public string Ingredient {  set; get; }
        public float? TargetQuantity { get; set; }
        public float? TargetQuantityMax { get; set; }

        public string? FinalQuantity { get; set; }

        public string? FinalQuantityMax { get; set; }
        public string TargetUnit { get; set; }
        
        public string TargetUnitAlternative { get; set; }

        //public int? SortOrder { get; set; }

        //public bool IsActive { get; set; }

    }



}
