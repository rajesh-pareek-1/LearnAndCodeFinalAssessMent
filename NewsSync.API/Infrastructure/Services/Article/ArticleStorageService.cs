using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;

public class ArticleStorageService : IArticleStorageService
{
    private readonly IArticleRepository articleRepository;
    private readonly IServerRepository serverRepository;
    private readonly ILogger<ArticleStorageService> logger;

    public ArticleStorageService(IArticleRepository articleRepository, IServerRepository serverRepository, ILogger<ArticleStorageService> logger)
    {
        this.articleRepository = articleRepository;
        this.serverRepository = serverRepository;
        this.logger = logger;
    }

    public async Task StoreArticlesAsync(ServerDetail server, List<Article> articles)
    {
        if (articles == null || articles.Count == 0) return;

        await articleRepository.AddArticlesAsync(articles);
        await articleRepository.SaveChangesAsync();

        server.LastAccess = DateTime.UtcNow;
        await serverRepository.UpdateAsync(server);
        await serverRepository.SaveChangesAsync();

        logger.LogInformation("Stored {Count} articles from {Server}", articles.Count, server.ServerName);
    }
}
