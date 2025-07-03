namespace NewsSync.API.Application.DTOs
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public DateTime SentAt { get; set; }
        public ArticleDto Article { get; set; } = null!;
    }
}
