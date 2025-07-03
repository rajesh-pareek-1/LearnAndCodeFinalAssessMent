using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class ServerUpdateRequestDto
    {
        [Required(ErrorMessage = "API key is required")]
        [MinLength(10, ErrorMessage = "API key must be at least 10 characters long")]
        [MaxLength(100, ErrorMessage = "API key must not exceed 100 characters")]
        public string NewApiKey { get; set; } = string.Empty;
    }
}
