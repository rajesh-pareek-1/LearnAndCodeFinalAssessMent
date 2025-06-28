namespace NewsSync.API.Application.DTOs
{
    public class ReactionRequestDto
    {
        public int ArticleId { get; set; }
        public string UserId { get; set; } = null!;
        public bool IsLiked { get; set; }
    }
}
