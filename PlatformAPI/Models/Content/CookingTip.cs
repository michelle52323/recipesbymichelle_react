namespace PlatformAPI.Models.Content
{
    public class CookingTip
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string? Category { get; set; }

        public string? Season { get; set; }
    }
}
