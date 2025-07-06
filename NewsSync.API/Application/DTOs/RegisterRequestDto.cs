using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username (email) is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be in a valid format like user@example.com")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character")]
        public string Password { get; set; } = string.Empty;
    }
}
