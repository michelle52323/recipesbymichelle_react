namespace PlatformAPI.Models.Categories
{
    public class Category
    {

        public int UserId { get; set; }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
