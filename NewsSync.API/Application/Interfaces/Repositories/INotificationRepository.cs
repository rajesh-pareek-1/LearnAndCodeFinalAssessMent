using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task<List<NotificationConfiguration>> GetNotificationSettingsAsync(string userId);
        Task AddNotificationConfigurationAsync(NotificationConfiguration config);
        Task RemoveNotificationConfigurationAsync(NotificationConfiguration config);
        Task<NotificationConfiguration?> GetNotificationConfigurationAsync(string userId, int categoryId);
        Task<Category?> GetCategoryByNameAsync(string categoryName);
        Task SaveChangesAsync();
    }
}

