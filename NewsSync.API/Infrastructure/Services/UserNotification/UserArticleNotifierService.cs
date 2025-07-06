using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

public class UserArticleNotifierService : IUserArticleNotifierService
{
    private readonly NewsSyncNewsDbContext newsDb;
    private readonly NewsSyncAuthDbContext authDb;
    private readonly IUserNotificationService notificationService;
    private readonly ILogger<UserArticleNotifierService> logger;

    public UserArticleNotifierService(NewsSyncNewsDbContext newsDb, NewsSyncAuthDbContext authDb, IUserNotificationService notificationService, ILogger<UserArticleNotifierService> logger)
    {
        this.newsDb = newsDb;
        this.authDb = authDb;
        this.notificationService = notificationService;
        this.logger = logger;
    }

    public async Task NotifyUsersAsync(List<Article> articles)
    {
        var articlesByCategory = GroupArticlesByCategory(articles);

        foreach (var categoryGroup in articlesByCategory)
        {
            var categoryId = categoryGroup.Key;
            var articleList = categoryGroup.ToList();

            var userIds = await GetUserIdsSubscribedToCategoryAsync(categoryId);
            var users = await GetNotifiableUsersAsync(userIds);

            foreach (var user in users)
            {
                try
                {
                    var emailBody = ComposeEmailBody(articleList);
                    await notificationService.SendEmailAsync(
                        user.Email,
                        $"📰 New Articles in Category {categoryId}",
                        emailBody
                    );

                    await SaveNotificationRecordsAsync(articleList, user.Id);

                    logger.LogInformation("Notified {UserEmail} for category {CategoryId}", user.Email, categoryId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to notify {UserEmail}", user.Email);
                }
            }
        }
    }

    private ILookup<int, Article> GroupArticlesByCategory(List<Article> articles)
    {
        return articles
            .Where(a => a.CategoryId > 0)
            .ToLookup(a => a.CategoryId);
    }

    private async Task<List<string>> GetUserIdsSubscribedToCategoryAsync(int categoryId)
    {
        return await newsDb.NotificationConfigurations
            .Where(cfg => cfg.CategoryId == categoryId)
            .Select(cfg => cfg.UserId)
            .Distinct()
            .ToListAsync();
    }

    private async Task<List<(string Email, string Id)>> GetNotifiableUsersAsync(List<string> userIds)
    {
        return await authDb.Users
            .Where(u => userIds.Contains(u.Id) && !string.IsNullOrWhiteSpace(u.Email))
            .Select(u => new ValueTuple<string, string>(u.Email, u.Id))
            .ToListAsync();
    }

    private string ComposeEmailBody(List<Article> articles)
    {
        return string.Join("\n\n", articles.Select(a =>
            $"🔹 {a.Headline}\n{a.Description}\n{a.Url}"
        ));
    }

    private async Task SaveNotificationRecordsAsync(List<Article> articles, string userId)
    {
        var notifications = articles.Select(a => new Notification
        {
            ArticleId = a.Id,
            UserId = userId,
            SentAt = DateTime.UtcNow
        });

        newsDb.Notifications.AddRange(notifications);
        await newsDb.SaveChangesAsync();
    }
}
