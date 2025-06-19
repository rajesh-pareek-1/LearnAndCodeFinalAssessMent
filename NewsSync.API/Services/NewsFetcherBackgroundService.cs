// Services/NewsFetcherBackgroundService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;
using NewsSync.API.Repositories;
using NewsSync.API.Services;

public class NewsFetcherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NewsFetcherBackgroundService> _logger;
    private readonly Dictionary<string, INewsAdapter> _adapters;

    public NewsFetcherBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<NewsFetcherBackgroundService> logger,
        Dictionary<string, INewsAdapter> adapters)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _adapters = adapters;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await FetchAndStoreArticlesAsync();
            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }

    private async Task FetchAndStoreArticlesAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NewsSyncNewsDbContext>();

        var servers = await db.ServerDetails.ToListAsync();
        foreach (var server in servers)
        {
            if (!_adapters.TryGetValue(server.ServerName, out var adapter))
            {
                _logger.LogWarning("No adapter found for server: {ServerName}", server.ServerName);
                continue;
            }

            try
            {
                var articles = await adapter.FetchArticlesAsync(server.BaseUrl, server.ApiKey);
                db.Articles.AddRange(articles);
                server.LastAccess = DateTime.UtcNow;
                await db.SaveChangesAsync();

                _logger.LogInformation("Fetched {Count} articles from {Server}", articles.Count, server.ServerName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch articles from {Server}", server.ServerName);
            }
        }
    }
}
