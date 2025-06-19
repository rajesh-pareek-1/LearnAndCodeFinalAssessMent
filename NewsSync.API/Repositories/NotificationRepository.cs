using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;

public class NotificationRepository : INotificationRepository
{
    private readonly NewsSyncNewsDbContext db;

    public NotificationRepository(NewsSyncNewsDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
    {
        return await db.Notifications
            .Include(n => n.Article)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<List<NotificationConfiguration>> GetNotificationSettingsAsync(string userId)
    {
        return await db.NotificationConfigurations
            .Include(nc => nc.Category)
            .Where(nc => nc.UserId == userId)
            .ToListAsync();
    }

    public async Task AddNotificationConfigurationAsync(NotificationConfiguration config)
    {
        await db.NotificationConfigurations.AddAsync(config);
    }

    public Task RemoveNotificationConfigurationAsync(NotificationConfiguration config)
    {
        db.NotificationConfigurations.Remove(config);
        return Task.CompletedTask;
    }

    public async Task<NotificationConfiguration?> GetNotificationConfigurationAsync(string userId, int categoryId)
    {
        return await db.NotificationConfigurations
            .FirstOrDefaultAsync(nc => nc.UserId == userId && nc.CategoryId == categoryId);
    }

    public async Task<Category?> GetCategoryByNameAsync(string categoryName)
    {
        return await db.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
    }

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
