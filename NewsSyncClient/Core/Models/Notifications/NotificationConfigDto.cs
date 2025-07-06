using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Core.Models.Notifications;

public class NotificationConfigDto
{
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; } = new();
}
