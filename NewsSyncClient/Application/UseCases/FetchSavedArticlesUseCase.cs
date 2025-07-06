using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Application.UseCases;

public class FetchSavedArticlesUseCase : IFetchSavedArticlesUseCase
{
    private readonly ISavedArticleService _service;

    public FetchSavedArticlesUseCase(ISavedArticleService service) => _service = service;

    public Task<List<ArticleDto>> ExecuteAsync() => _service.GetSavedArticlesAsync();
}
