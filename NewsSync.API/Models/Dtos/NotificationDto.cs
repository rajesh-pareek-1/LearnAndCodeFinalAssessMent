namespace NewsSync.API.Models.DTO
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public DateTime SentAt { get; set; }
        public ArticleDto Article { get; set; } = null!;
    }
}




