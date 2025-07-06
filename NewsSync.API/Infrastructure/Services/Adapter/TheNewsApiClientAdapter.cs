using System.Text.Json;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Services;

public class TheNewsApiClientAdapter : INewsAdapter
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TheNewsApiClientAdapter> logger;

    public TheNewsApiClientAdapter(HttpClient httpClient, ILogger<TheNewsApiClientAdapter> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey, List<CategoryResponseDto> categories)
    {
        try
        {
            var url = BuildRequestUrl(baseUrl, apiKey);
            var request = BuildHttpRequest(url);
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return ParseArticles(json, categories);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch articles from TheNewsAPI");
            return [];
        }
    }

    private static string BuildRequestUrl(string baseUrl, string apiKey)
    {
        return $"{baseUrl}?api_token={Uri.EscapeDataString(apiKey)}&locale=us&limit=10";
    }

    private static HttpRequestMessage BuildHttpRequest(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "NewsSyncClient/1.0");
        return request;
    }

    private static List<Article> ParseArticles(string json, List<CategoryResponseDto> categories)
    {
        var articles = new List<Article>();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
            return articles;

        foreach (var item in data.EnumerateArray())
        {
            articles.Add(MapToArticle(item, categories));
        }

        return articles;
    }

    private static Article MapToArticle(JsonElement item, List<CategoryResponseDto> categories)
    {
        var article = new Article
        {
            Headline = item.GetProperty("title").GetString() ?? "",
            Description = item.GetProperty("description").GetString() ?? "",
            Source = item.GetProperty("source").GetString() ?? "Unknown",
            Url = item.GetProperty("url").GetString() ?? "",
            AuthorName = item.TryGetProperty("author", out var author) ? author.GetString() ?? "Unknown" : "Unknown",
            ImageUrl = item.GetProperty("image_url").GetString() ?? "",
            Language = item.GetProperty("language").GetString() ?? "en",
            PublishedDate = item.GetProperty("published_at").GetString() ?? DateTime.UtcNow.ToString("o")
        };

        var matchedCategory = ArticleCategoryPredictor.PredictCategory(article, categories);
        article.CategoryId = matchedCategory.Id;

        return article;
    }
}
