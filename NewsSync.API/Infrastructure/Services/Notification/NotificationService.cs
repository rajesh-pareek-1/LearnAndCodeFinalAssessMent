using NewsSync.API.Application.Exceptions;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly ILogger<NotificationService> logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            this.notificationRepository = notificationRepository;
            this.logger = logger;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                return await notificationRepository.GetUserNotificationsAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch notifications for user {UserId}", userId);
                throw new ApplicationException(ValidationMessages.FailedToFetchNotifications, ex);
            }
        }

        public async Task<List<NotificationConfiguration>> GetSettingsAsync(string userId)
        {
            try
            {
                return await notificationRepository.GetNotificationSettingsAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get notification settings for user {UserId}", userId);
                throw new ApplicationException(ValidationMessages.FailedToFetchSettings, ex);
            }
        }

        public async Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled)
        {
            try
            {
                var category = await EnsureCategoryExists(categoryName);
                var existingConfig = await notificationRepository.GetNotificationConfigurationAsync(userId, category.Id);

                if (enabled && existingConfig == null)
                {
                    await AddNewConfig(userId, category.Id);
                }
                else if (!enabled && existingConfig != null)
                {
                    await RemoveConfig(existingConfig);
                }

                await notificationRepository.SaveChangesAsync();
                return true;
            }
            catch (CategoryNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update notification setting for user {UserId}, category {Category}", userId, categoryName);
                throw new ApplicationException(ValidationMessages.FailedToUpdateNotificationSetting, ex);
            }
        }

        private async Task<Category> EnsureCategoryExists(string categoryName)
        {
            var category = await notificationRepository.GetCategoryByNameAsync(categoryName) ?? throw new CategoryNotFoundException(categoryName);
            return category;
        }

        private Task AddNewConfig(string userId, int categoryId) =>
            notificationRepository.AddNotificationConfigurationAsync(new NotificationConfiguration
            {
                UserId = userId,
                CategoryId = categoryId
            });

        private Task RemoveConfig(NotificationConfiguration config) =>
            notificationRepository.RemoveNotificationConfigurationAsync(config);
    }
}
