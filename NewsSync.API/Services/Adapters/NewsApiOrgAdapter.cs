// Services/Adapters/NewsApiOrgAdapter.cs
using System.Net.Http.Json;
using System.Text.Json;
using NewsSync.API.Models.Domain;
using NewsSync.API.Services;

public class NewsApiOrgAdapter : INewsAdapter
{
    private readonly HttpClient _http;

    public NewsApiOrgAdapter(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey)
    {
        var url = $"{baseUrl}?q=latest&sortBy=publishedAt&apiKey={Uri.EscapeDataString(apiKey)}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "NewsSyncClient/1.0"); // <-- REQUIRED

        var response = await _http.SendAsync(request);

        var rawJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine("DEBUG RAW JSON: " + rawJson);

        response.EnsureSuccessStatusCode();

        var parsed = JsonSerializer.Deserialize<NewsApiOrgResponse>(rawJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return parsed?.Articles?.Select(a => new Article
        {
            Headline = a.Title ?? "",
            Description = a.Description ?? "",
            Source = a.Source?.Name ?? "Unknown",
            Url = a.Url ?? "",
            AuthorName = a.Author ?? "",
            ImageUrl = a.UrlToImage ?? "",
            Language = "en",
            PublishedDate = a.PublishedAt ?? "",
            CategoryId = 1
        }).ToList() ?? new List<Article>();
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
