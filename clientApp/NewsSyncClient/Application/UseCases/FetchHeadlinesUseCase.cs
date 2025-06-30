using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Application.UseCases;

public class FetchHeadlinesUseCase : IFetchHeadlinesUseCase
{
    private readonly IArticleInteractionService _articleInteractionService;
    private readonly ISessionContext _sessionContext;

    public FetchHeadlinesUseCase(IArticleInteractionService articleInteractionService, ISessionContext sessionContext) =>
        (_articleInteractionService, _sessionContext) = (articleInteractionService, sessionContext);

    public async Task<List<ArticleDto>> ExecuteAsync(DateTime fromDate, DateTime toDate, string? category)
    {
        var fetchedArticles = await _articleInteractionService.FetchHeadlinesAsync(fromDate, toDate, category);
        if (_sessionContext.UserId is null || !fetchedArticles.Any()) return fetchedArticles;

        var likedArticleIds = (await _articleInteractionService.GetUserReactionsAsync(true)).Select(article => article.Id).ToHashSet();
        var dislikedArticleIds = (await _articleInteractionService.GetUserReactionsAsync(false)).Select(article => article.Id).ToHashSet();

        fetchedArticles.ForEach(article =>
        {
            article.IsLiked = likedArticleIds.Contains(article.Id);
            article.IsDisliked = dislikedArticleIds.Contains(article.Id);
        });

        return fetchedArticles;
    }

    public Task<List<CategoryDto>> GetCategoriesAsync() => _articleInteractionService.FetchCategoriesAsync();
}
