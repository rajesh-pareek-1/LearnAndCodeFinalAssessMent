using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;

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

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                var notifications = await notificationRepository.GetUserNotificationsAsync(userId);

                return notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    SentAt = n.SentAt,
                    Article = new ArticleDto
                    {
                        Id = n.Article.Id,
                        Headline = n.Article.Headline,
                        Description = n.Article.Description,
                        Source = n.Article.Source,
                        Url = n.Article.Url,
                        AuthorName = n.Article.AuthorName,
                        ImageUrl = n.Article.ImageUrl,
                        Language = n.Article.Language,
                        PublishedDate = n.Article.PublishedDate
                    }
                }).ToList();
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
                var category = await notificationRepository.GetCategoryByNameAsync(categoryName);
                if (category == null)
                    return false;

                var existingConfig = await notificationRepository.GetNotificationConfigurationAsync(userId, category.Id);

                if (enabled && existingConfig == null)
                {
                    await notificationRepository.AddNotificationConfigurationAsync(new NotificationConfiguration
                    {
                        UserId = userId,
                        CategoryId = category.Id
                    });
                }

                if (!enabled && existingConfig != null)
                {
                    await notificationRepository.RemoveNotificationConfigurationAsync(existingConfig);
                }

                await notificationRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update notification setting for user {UserId}, category {Category}", userId, categoryName);
                throw new ApplicationException(ValidationMessages.FailedToUpdateNotificationSetting, ex);
            }
        }
    }
}
