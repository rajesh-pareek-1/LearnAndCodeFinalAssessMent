using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task<List<NotificationConfiguration>> GetSettingsAsync(string userId);
        Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled);
    }
}
