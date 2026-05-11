using System.ComponentModel.DataAnnotations;
using PlatformAPI.Models.Users;
namespace PlatformAPI.Models.Recipe
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public int SortOrder { get; set; }

        public bool IsMine { get; set; }

        // Optional navigation properties
        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
