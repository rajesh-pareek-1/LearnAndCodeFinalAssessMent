using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Infrastructure.Data;
using NewsSync.API.Application.DTOs;

public class NewsFetcherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<NewsFetcherBackgroundService> logger;
    private readonly Dictionary<string, INewsAdapter> adapters;

    public NewsFetcherBackgroundService(IServiceScopeFactory scopeFactory, ILogger<NewsFetcherBackgroundService> logger, Dictionary<string, INewsAdapter> adapters)
    {
        this.scopeFactory = scopeFactory;
        this.logger = logger;
        this.adapters = adapters;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("News fetcher started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PerformFetchCycleAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ValidationMessages.FailedToFetchArticles);
            }

            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }

    private async Task PerformFetchCycleAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var newsDb = scope.ServiceProvider.GetRequiredService<NewsSyncNewsDbContext>();
        var articleStorage = scope.ServiceProvider.GetRequiredService<IArticleStorageService>();
        var notifier = scope.ServiceProvider.GetRequiredService<IUserArticleNotifierService>();
        var articleCategoryService = scope.ServiceProvider.GetRequiredService<IArticleCategoryService>();

        var articleCategories = await articleCategoryService.GetAllCategoriesAsync();

        var servers = await LoadAllServersAsync(newsDb);

        foreach (var server in servers)
        {
            if (!TryGetAdapter(server.ServerName, out var adapter)) continue;

            var articles = await FetchArticlesAsync(adapter, server, articleCategories);
            if (articles.Count == 0) continue;

            await StoreArticlesAsync(articleStorage, server, articles);
            await NotifyUsersAsync(notifier, articles);
        }

        var preferenceBuilder = scope.ServiceProvider.GetRequiredService<IUserPreferenceBuilderService>();
        await preferenceBuilder.UpdateAllUserPreferencesAsync();

    }

    private async Task<List<ServerDetail>> LoadAllServersAsync(NewsSyncNewsDbContext dbContext)
    {
        return await dbContext.ServerDetails.ToListAsync();
    }

    private bool TryGetAdapter(string serverName, out INewsAdapter adapter)
    {
        if (adapters.TryGetValue(serverName, out adapter)) return true;

        logger.LogWarning("No adapter configured for server: {Server}", serverName);
        return false;
    }

    private async Task<List<Article>> FetchArticlesAsync(INewsAdapter adapter, ServerDetail server, List<CategoryResponseDto> categories)
    {
        try
        {
            var articles = await adapter.FetchArticlesAsync(server.BaseUrl, server.ApiKey, categories);
            logger.LogInformation("Fetched {Count} articles from {Server}", articles.Count, server.ServerName);
            return articles;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch articles from {Server}", server.ServerName);
            return [];
        }
    }

    private async Task StoreArticlesAsync(IArticleStorageService articleStorage, ServerDetail server, List<Article> articles)
    {
        try
        {
            await articleStorage.StoreArticlesAsync(server, articles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to store articles for {Server}", server.ServerName);
        }
    }

    private async Task NotifyUsersAsync(IUserArticleNotifierService notifier, List<Article> articles)
    {
        try
        {
            //await notifier.NotifyUsersAsync(articles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to notify users for articles");
        }
    }
}
