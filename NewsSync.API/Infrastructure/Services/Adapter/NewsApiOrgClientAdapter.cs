using System.Text.Json;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Services;

public class NewsApiOrgClientAdapter : INewsAdapter
{
    private readonly HttpClient httpClient;
    private readonly ILogger<NewsApiOrgClientAdapter> logger;

    public NewsApiOrgClientAdapter(HttpClient httpClient, ILogger<NewsApiOrgClientAdapter> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey, List<CategoryResponseDto> categories)
    {
        var requestUrl = BuildRequestUrl(baseUrl, apiKey);
        var request = BuildHttpRequest(requestUrl);

        try
        {
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return ParseArticlesFromJson(json, categories);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch articles from NewsAPI.org");
            return [];
        }
    }

    private static string BuildRequestUrl(string baseUrl, string apiKey)
    {
        return $"{baseUrl}?q=latest&sortBy=publishedAt&apiKey={Uri.EscapeDataString(apiKey)}";
    }

    private static HttpRequestMessage BuildHttpRequest(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "NewsSyncClient/1.0");
        return request;
    }

    private static List<Article> ParseArticlesFromJson(string json, List<CategoryResponseDto> categories)
    {
        var parsed = JsonSerializer.Deserialize<NewsApiOrgResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return parsed?.Articles?.Select(a => MapToArticle(a, categories)).ToList() ?? [];
    }

    private static Article MapToArticle(NewsApiOrgResponse.NewsApiArticle a, List<CategoryResponseDto> categories)
    {
        var article = new Article
        {
            Headline = a.Title ?? "",
            Description = a.Description ?? "",
            Source = a.Source?.Name ?? "Unknown",
            Url = a.Url ?? "",
            AuthorName = a.Author ?? "",
            ImageUrl = a.UrlToImage ?? "",
            Language = "en",
            PublishedDate = a.PublishedAt ?? ""
        };

        var matchedCategory = ArticleCategoryPredictor.PredictCategory(article, categories);
        article.CategoryId = matchedCategory.Id;

        return article;

    }

    private class NewsApiOrgResponse
    {
        public List<NewsApiArticle>? Articles { get; set; }

        public class NewsApiArticle
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Url { get; set; }
            public string? Author { get; set; }
            public string? UrlToImage { get; set; }
            public string? PublishedAt { get; set; }
            public SourceObj? Source { get; set; }

            public class SourceObj
            {
                public string? Name { get; set; }
            }
        }
    }
}
