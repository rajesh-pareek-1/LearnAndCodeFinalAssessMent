using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Models.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public DateTime SentAt { get; set; }
    public ArticleDto Article { get; set; } = new();
}
