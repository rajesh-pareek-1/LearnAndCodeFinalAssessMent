using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Application.Services;

public class SearchArticleService : ISearchArticleService
{
    private readonly IApiClient _apiClient;

    public SearchArticleService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<ArticleDto>> SearchAsync(string query)
    {
        var encodedQuery = Uri.EscapeDataString(query);
        return _apiClient.GetAsync<List<ArticleDto>>($"/api/article/search?query={encodedQuery}");
    }
}
