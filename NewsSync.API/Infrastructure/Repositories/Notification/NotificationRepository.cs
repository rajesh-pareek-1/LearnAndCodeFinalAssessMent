using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

public class NotificationRepository : INotificationRepository
{
    private readonly NewsSyncNewsDbContext _newsDbContext;

    public NotificationRepository(NewsSyncNewsDbContext _newsDbContext)
    {
        this._newsDbContext = _newsDbContext;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
    {
        return await _newsDbContext.Notifications
            .Include(n => n.Article)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<List<NotificationConfiguration>> GetNotificationSettingsAsync(string userId)
    {
        return await _newsDbContext.NotificationConfigurations
            .Include(nc => nc.Category)
            .Where(nc => nc.UserId == userId)
            .ToListAsync();
    }

    public async Task AddNotificationConfigurationAsync(NotificationConfiguration config)
    {
        await _newsDbContext.NotificationConfigurations.AddAsync(config);
    }

    public Task RemoveNotificationConfigurationAsync(NotificationConfiguration config)
    {
        _newsDbContext.NotificationConfigurations.Remove(config);
        return Task.CompletedTask;
    }

    public async Task<NotificationConfiguration?> GetNotificationConfigurationAsync(string userId, int categoryId)
    {
        return await _newsDbContext.NotificationConfigurations
            .FirstOrDefaultAsync(nc => nc.UserId == userId && nc.CategoryId == categoryId);
    }

    public async Task<Category?> GetCategoryByNameAsync(string categoryName)
    {
        return await _newsDbContext.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
    }

    public Task SaveChangesAsync() => _newsDbContext.SaveChangesAsync();
}
