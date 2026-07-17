namespace PlatformAPI.Models.Categories
{
    public class RecipeCategory
    {

        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
        public bool IsMine { get; set; }
        public int SortOrder { get; set; }

        public Category? Category { get; set; }

    }
}
