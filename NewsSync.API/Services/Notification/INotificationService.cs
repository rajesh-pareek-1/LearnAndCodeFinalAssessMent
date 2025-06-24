using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task<List<NotificationConfiguration>> GetSettingsAsync(string userId);
        Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled);
    }
}
