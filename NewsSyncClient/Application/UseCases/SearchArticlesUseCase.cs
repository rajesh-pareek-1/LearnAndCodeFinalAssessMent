using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Application.UseCases;

public class SearchArticlesUseCase : ISearchArticlesUseCase
{
    private readonly ISearchArticleService _searchService;

    public SearchArticlesUseCase(ISearchArticleService searchService)
    {
        _searchService = searchService;
    }

    public async Task<List<ArticleDto>> ExecuteAsync(string query)
    {
        return await _searchService.SearchAsync(query);
    }
}
