using System.Net.Http.Json;
using System.Text.Json;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Application.Services;

public class ArticleInteractionService : IArticleInteractionService
{
    private readonly HttpClient _client;
    private readonly ISessionContext _session;

    public ArticleInteractionService(IHttpClientProvider clientProvider, ISessionContext session)
    {
        _client = clientProvider.Client;
        _session = session;
    }

    public async Task<List<ArticleDto>> FetchHeadlinesAsync(DateTime from, DateTime to, string? category = null)
    {
        var url = $"/api/article?fromDate={from:yyyy-MM-dd}&toDate={to.AddDays(1):yyyy-MM-dd}";
        if (!string.IsNullOrWhiteSpace(category))
            url += $"&category={Uri.EscapeDataString(category)}";

        var response = await _client.GetAsync(url);
        Console.WriteLine($"[Article Fetch] URL: {url}");
        Console.WriteLine($"[Article Fetch] Status: {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
            return [];

        var articles = await response.Content.ReadFromJsonAsync<List<ArticleDto>>() ?? [];

        // ✅ Enrich with user-specific reactions
        if (_session.UserId is not null && articles.Any())
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

        // ✅ Log enriched articles
        var json = JsonSerializer.Serialize(articles, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine("[Final Enriched Articles]:\n" + json);

        return articles;
    }

    public async Task<List<CategoryDto>> FetchCategoriesAsync()
    {
        var response = await _client.GetAsync("/api/categories/article");
        if (!response.IsSuccessStatusCode)
            return new();

        return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new();
    }

    public async Task SaveArticleAsync(int articleId)
    {
        var payload = new { articleId, userId = _session.UserId };
        await _client.PostAsJsonAsync("/api/savedArticle", payload);
    }

    public async Task ReactToArticleAsync(int articleId, bool isLiked)
    {
        var payload = new
        {
            ArticleId = articleId,
            UserId = _session.UserId,
            IsLiked = isLiked
        };
        await _client.PostAsJsonAsync("/api/article/reaction", payload);
    }

    public async Task ReportArticleAsync(int articleId, string? reason)
    {
        var payload = new
        {
            ArticleId = articleId,
            UserId = _session.UserId,
            Reason = reason
        };
        await _client.PostAsJsonAsync("/api/article/report", payload);
    }

    public async Task<List<ArticleDto>> GetUserReactionsAsync(bool liked)
    {
        if (_session.UserId is null)
            return new();

        var url = $"/api/Article/reaction/user/{_session.UserId}?liked={liked.ToString().ToLower()}";

        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ArticleDto>>() ?? [];
    }

}
