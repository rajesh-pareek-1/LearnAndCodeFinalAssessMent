using System.Net.Http;
using System.Text.Json;
using NewsSync.API.Models.Domain;
using NewsSync.API.Services;

public class TheNewsApiAdapter : INewsAdapter
{
    private readonly HttpClient httpClient;

    public TheNewsApiAdapter(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey)
    {
        var url = $"{baseUrl}?api_token={Uri.EscapeDataString(apiKey)}&locale=us&limit=10";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "NewsSyncClient/1.0");

        var response = await httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        Console.WriteLine("RAW JSON (TheNewsAPI):");
        Console.WriteLine(json);

        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("data", out var articlesJson) || articlesJson.ValueKind != JsonValueKind.Array)
        {
            Console.WriteLine("⚠️ 'data' property missing or not an array in TheNewsAPI response.");
            return new List<Article>();
        }

        var articles = new List<Article>();

        foreach (var item in articlesJson.EnumerateArray())
        {
            articles.Add(new Article
            {
                Headline = item.GetProperty("title").GetString() ?? "",
                Description = item.GetProperty("description").GetString() ?? "",
                Source = item.GetProperty("source").GetString() ?? "",
                Url = item.GetProperty("url").GetString() ?? "",
                AuthorName = item.TryGetProperty("author", out var author) ? author.GetString() ?? "Unknown" : "Unknown",
                ImageUrl = item.GetProperty("image_url").GetString() ?? "",
                Language = item.GetProperty("language").GetString() ?? "en",
                PublishedDate = item.GetProperty("published_at").GetString() ?? DateTime.UtcNow.ToString("o"),
                CategoryId = 1
            });
        }

        return articles;
    }

}
