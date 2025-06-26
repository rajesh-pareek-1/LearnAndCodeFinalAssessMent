namespace NewsSync.API.Models.DTO
{
    public class SaveArticleRequestDto
    {
        public int ArticleId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
