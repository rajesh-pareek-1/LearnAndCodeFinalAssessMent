using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username (email) is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be a valid format (e.g., user@example.com)")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long with uppercase, lowercase, digit, and special character")]
        public string Password { get; set; } = string.Empty;
    }
}
