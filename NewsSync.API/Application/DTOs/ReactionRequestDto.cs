using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Application.DTOs
{
    public class ReactionRequestDto
    {
        [Required(ErrorMessage = "ArticleId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ArticleId must be greater than zero")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        [MinLength(5, ErrorMessage = "UserId must be at least 5 characters long")]
        public string UserId { get; set; } = string.Empty;

        public bool IsLiked { get; set; }
    }
}
