using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class NotificationSettingUpdateRequestDto
    {
        [Required(ErrorMessage = "UserId is required")]
        [MinLength(5, ErrorMessage = "UserId must be at least 5 characters long")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "CategoryName is required")]
        [MinLength(2, ErrorMessage = "CategoryName must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "CategoryName must not exceed 50 characters")]
        public string CategoryName { get; set; } = string.Empty;

        public bool Enabled { get; set; }
    }
}
