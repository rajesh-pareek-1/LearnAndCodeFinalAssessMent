using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class ReportDto
    {
        [Required(ErrorMessage = "ArticleId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ArticleId must be a positive number")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        [MinLength(5, ErrorMessage = "UserId must be at least 5 characters long")]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Reason must not exceed 500 characters")]
        public string? Reason { get; set; }
    }
}
