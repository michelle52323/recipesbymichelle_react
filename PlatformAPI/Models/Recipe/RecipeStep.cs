namespace PlatformAPI.Models.Recipe
{
    public class RecipeStep
    {
        public int RecipeId { get; set; }  // FK to Recipe
        public int StepId { get; set; }    // FK to Step

    }
}
