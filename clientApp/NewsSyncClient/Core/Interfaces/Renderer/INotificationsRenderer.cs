using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Core.Interfaces.Renderer;

public interface INotificationsRenderer
{
    void RenderHeader();
    void RenderNotifications(List<NotificationDto> notifications);
    void RenderCategories(List<NotificationCategoryDto> categories);
}
