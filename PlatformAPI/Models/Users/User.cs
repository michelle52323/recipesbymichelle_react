using System.ComponentModel.DataAnnotations;

namespace PlatformAPI.Models.Users
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? LegacyPassword { get; set; }

        public bool IsLegacyPassword { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Pronouns { get; set; }

        public int? GenderId { get; set; }

        public int UserTypeId { get; set; }

        public int ThemeId { get; set; }

        public Enums.MeasurementSystem MeasurementSystem { get; set; }
        public bool HasSelectedMeasurementSystem { get; set; }

        public Guid DeviceId { get; set; } // Unique identifier for device id

        public DateTime CreatedAt { get; set; }

        public UserType UserType { get; set; }

        public Gender? Gender { get; set; }


    }
}
