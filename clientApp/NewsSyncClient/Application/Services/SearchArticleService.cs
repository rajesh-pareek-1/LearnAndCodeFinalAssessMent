using NewsSyncClient.Core.Exceptions;
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
        if (string.IsNullOrWhiteSpace(query))
            throw new ValidationException("Search query cannot be empty.");

        var encodedQuery = Uri.EscapeDataString(query);
        return _apiClient.GetAsync<List<ArticleDto>>($"/api/article/search?query={encodedQuery}");
    }
}
