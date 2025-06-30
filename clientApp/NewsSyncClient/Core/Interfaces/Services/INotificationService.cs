using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Core.Interfaces.Services;

public interface INotificationService
{
    Task<List<NotificationDto>> GetNotificationsAsync();
    Task<List<CategoryDto>> GetNotificationCategoriesAsync();
    Task<bool> ConfigureNotificationAsync(string categoryName, bool enabled);
}
