using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Application.Services;

public class ArticleInteractionService : IArticleInteractionService
{
    private readonly IApiClient _apiClient;
    private readonly ISessionContext _session;

    public ArticleInteractionService(IApiClient apiClient, ISessionContext session)
    {
        _apiClient = apiClient;
        _session = session;
    }

    public async Task<List<ArticleDto>> FetchHeadlinesAsync(DateTime from, DateTime to, string? category = null)
    {
        var url = $"/api/article?fromDate={from:yyyy-MM-dd}&toDate={to.AddDays(1):yyyy-MM-dd}";
        if (!string.IsNullOrWhiteSpace(category))
            url += $"&category={Uri.EscapeDataString(category)}";

        var articles = await _apiClient.GetAsync<List<ArticleDto>>(url);

        if (_session.UserId is not null && articles.Count != 0)
        {
            var likedArticles = await GetUserReactionsAsync(true);
            var dislikedArticles = await GetUserReactionsAsync(false);

            var likedIds = likedArticles.Select(a => a.Id).ToHashSet();
            var dislikedIds = dislikedArticles.Select(a => a.Id).ToHashSet();

            foreach (var article in articles)
            {
                article.IsLiked = likedIds.Contains(article.Id);
                article.IsDisliked = dislikedIds.Contains(article.Id);
            }
        }

        return articles;
    }

    public Task<List<CategoryDto>> FetchCategoriesAsync()
    {
        return _apiClient.GetAsync<List<CategoryDto>>("/api/categories/article");
    }

    public Task SaveArticleAsync(int articleId)
    {
        var payload = new { articleId, userId = _session.UserId };
        return _apiClient.PostAsync("/api/savedArticle", payload);
    }

    public Task ReactToArticleAsync(int articleId, bool isLiked)
    {
        var payload = new
        {
            ArticleId = articleId,
            _session.UserId,
            IsLiked = isLiked
        };
        return _apiClient.PostAsync("/api/article/reaction", payload);
    }

    public Task ReportArticleAsync(int articleId, string? reason)
    {
        var payload = new
        {
            ArticleId = articleId,
            _session.UserId,
            Reason = reason
        };
        return _apiClient.PostAsync("/api/article/report", payload);
    }

    public async Task<List<ArticleDto>> GetUserReactionsAsync(bool liked)
    {
        if (_session.UserId is null)
            return [];

        var url = $"/api/article/reaction/user/{_session.UserId}?liked={liked.ToString().ToLower()}";
        return await _apiClient.GetAsync<List<ArticleDto>>(url);
    }
}
