using NewsSync.API.Models;
using NewsSync.API.Models.Domain;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository repo;

    public NotificationService(INotificationRepository repo)
    {
        this.repo = repo;
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
    {
        var notifications = await repo.GetUserNotificationsAsync(userId);

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

    public Task<List<NotificationConfiguration>> GetSettingsAsync(string userId)
    {
        return repo.GetNotificationSettingsAsync(userId);
    }

    public async Task<bool> UpdateSettingAsync(string userId, string categoryName, bool enabled)
    {
        var category = await repo.GetCategoryByNameAsync(categoryName);
        if (category == null)
            return false;

        var existing = await repo.GetNotificationConfigurationAsync(userId, category.Id);

        if (enabled && existing == null)
            await repo.AddNotificationConfigurationAsync(new NotificationConfiguration { UserId = userId, CategoryId = category.Id });

        if (!enabled && existing != null)
            await repo.RemoveNotificationConfigurationAsync(existing);

        await repo.SaveChangesAsync();
        return true;
    }
}
