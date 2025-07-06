using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public NotificationRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await newsDb.Notifications
                .Include(n => n.Article)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<NotificationConfiguration>> GetNotificationSettingsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await newsDb.NotificationConfigurations
                .Include(nc => nc.Category)
                .Where(nc => nc.UserId == userId)
                .ToListAsync();
        }

        public async Task AddNotificationConfigurationAsync(NotificationConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config);
            await newsDb.NotificationConfigurations.AddAsync(config);
        }

        public Task RemoveNotificationConfigurationAsync(NotificationConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config);
            newsDb.NotificationConfigurations.Remove(config);
            return Task.CompletedTask;
        }

        public async Task<NotificationConfiguration?> GetNotificationConfigurationAsync(string userId, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await newsDb.NotificationConfigurations
                .FirstOrDefaultAsync(notificationConfiguration => notificationConfiguration.UserId == userId && notificationConfiguration.CategoryId == categoryId);
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));

            return await newsDb.Categories
                .FirstOrDefaultAsync(category => category.Name.Equals(categoryName, StringComparison.CurrentCultureIgnoreCase));
        }

        public Task SaveChangesAsync()
        {
            return newsDb.SaveChangesAsync();
        }
    }
}
