using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
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

