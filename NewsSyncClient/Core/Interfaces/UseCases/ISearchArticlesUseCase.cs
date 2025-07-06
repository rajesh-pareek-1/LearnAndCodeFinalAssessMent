using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.UseCases;
public interface ISearchArticlesUseCase
{
    Task<List<ArticleDto>> ExecuteAsync(string query);
}
