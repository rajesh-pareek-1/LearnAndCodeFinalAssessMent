using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Application.UseCases;

public class FetchHeadlinesUseCase : IFetchHeadlinesUseCase
{
    private readonly IArticleInteractionService _articleService;
    private readonly ISessionContext _session;

    public FetchHeadlinesUseCase(IArticleInteractionService articleService, ISessionContext session)
    {
        _articleService = articleService;
        _session = session;
    }

    public async Task<List<ArticleDto>> ExecuteAsync(DateTime from, DateTime to, string? category)
    {
        var articles = await _articleService.FetchHeadlinesAsync(from, to, category);

        if (_session.UserId is not null && articles.Any())
        {
            var liked = await _articleService.GetUserReactionsAsync(true);
            var disliked = await _articleService.GetUserReactionsAsync(false);
            var likedSet = liked.Select(a => a.Id).ToHashSet();
            var dislikedSet = disliked.Select(a => a.Id).ToHashSet();

            foreach (var article in articles)
            {
                article.IsLiked = likedSet.Contains(article.Id);
                article.IsDisliked = dislikedSet.Contains(article.Id);
            }
        }

        return articles;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        return await _articleService.FetchCategoriesAsync();
    }
}
