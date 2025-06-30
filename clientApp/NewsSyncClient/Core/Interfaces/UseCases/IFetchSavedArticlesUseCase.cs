using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.UseCases;

public interface IFetchSavedArticlesUseCase
{
    Task<List<ArticleDto>> ExecuteAsync();
}
