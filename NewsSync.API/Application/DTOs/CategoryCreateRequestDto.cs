using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class CategoryCreateRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Description must not exceed 250 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
