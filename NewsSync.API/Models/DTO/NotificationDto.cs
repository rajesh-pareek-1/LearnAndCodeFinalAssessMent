namespace NewsSync.API.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public DateTime SentAt { get; set; }
        public ArticleDto Article { get; set; } = null!;
    }
}




