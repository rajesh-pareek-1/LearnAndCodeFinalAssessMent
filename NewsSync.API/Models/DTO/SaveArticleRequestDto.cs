namespace NewsSync.API.Models.DTOs
{
    public class SaveArticleRequestDto
    {
        public int ArticleId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
