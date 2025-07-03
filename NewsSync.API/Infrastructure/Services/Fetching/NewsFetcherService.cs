using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;

public class NewsFetcherService : INewsFetcherService
{
    private readonly Dictionary<string, INewsAdapter> adapters;
    private readonly ILogger<NewsFetcherService> logger;

    public NewsFetcherService(Dictionary<string, INewsAdapter> adapters, ILogger<NewsFetcherService> logger)
    {
        this.adapters = adapters;
        this.logger = logger;
    }

    public async Task<List<Article>> FetchArticlesAsync(ServerDetail server)
    {
        if (!adapters.TryGetValue(server.ServerName, out var adapter))
        {
            logger.LogWarning("No adapter configured for server: {ServerName}", server.ServerName);
            return new List<Article>();
        }

        return await adapter.FetchArticlesAsync(server.BaseUrl, server.ApiKey);
    }
}
