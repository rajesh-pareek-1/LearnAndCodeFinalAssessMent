using System.Net.Http.Json;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Application.Services;

public class SearchArticleService : ISearchArticleService
{
    private readonly HttpClient _client;

    public SearchArticleService(IHttpClientProvider clientProvider)
    {
        _client = clientProvider.Client;
    }

    public async Task<List<ArticleDto>> SearchAsync(string query)
    {
        var response = await _client.GetAsync($"/api/article/search?query={Uri.EscapeDataString(query)}");
        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ArticleDto>>() ?? [];
    }
}
