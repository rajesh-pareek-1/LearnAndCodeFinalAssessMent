using NewsSync.API.Models;
using NewsSync.API.Models.Domain;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
    Task<List<NotificationConfiguration>> GetSettingsAsync(string userId);
    Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled);
}
