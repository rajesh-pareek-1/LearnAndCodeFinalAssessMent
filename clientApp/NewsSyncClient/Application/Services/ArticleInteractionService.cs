using NewsSyncClient.Core.Exceptions;
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
        if (from > to)
            throw new ValidationException("Start date must be earlier than end date.");

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
        if (articleId <= 0)
            throw new ValidationException("Invalid article ID.");

        if (_session.UserId is null)
            throw new ValidationException("User must be logged in to save articles.");

        var payload = new { articleId, userId = _session.UserId };
        return _apiClient.PostAsync("/api/savedArticle", payload);
    }

    public Task ReactToArticleAsync(int articleId, bool isLiked)
    {
        if (articleId <= 0)
            throw new ValidationException("Invalid article ID.");

        if (_session.UserId is null)
            throw new ValidationException("User must be logged in to react to articles.");

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
        if (articleId <= 0)
            throw new ValidationException("Invalid article ID.");

        if (_session.UserId is null)
            throw new ValidationException("User must be logged in to report articles.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ValidationException("Report reason cannot be empty.");

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
