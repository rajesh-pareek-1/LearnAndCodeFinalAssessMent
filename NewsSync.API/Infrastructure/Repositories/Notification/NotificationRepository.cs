using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NewsSyncNewsDbContext db;

        public NotificationRepository(NewsSyncNewsDbContext db)
        {
            this.db = db;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await db.Notifications
                .Include(n => n.Article)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<NotificationConfiguration>> GetNotificationSettingsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await db.NotificationConfigurations
                .Include(nc => nc.Category)
                .Where(nc => nc.UserId == userId)
                .ToListAsync();
        }

        public async Task AddNotificationConfigurationAsync(NotificationConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            await db.NotificationConfigurations.AddAsync(config);
        }

        public Task RemoveNotificationConfigurationAsync(NotificationConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            db.NotificationConfigurations.Remove(config);
            return Task.CompletedTask;
        }

        public async Task<NotificationConfiguration?> GetNotificationConfigurationAsync(string userId, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await db.NotificationConfigurations
                .FirstOrDefaultAsync(nc => nc.UserId == userId && nc.CategoryId == categoryId);
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));

            return await db.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
