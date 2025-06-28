using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task<List<NotificationConfiguration>> GetSettingsAsync(string userId);
        Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled);
    }
}
