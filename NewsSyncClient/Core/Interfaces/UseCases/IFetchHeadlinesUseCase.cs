using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

namespace NewsSyncClient.Core.Interfaces.UseCases;

public interface IFetchHeadlinesUseCase
{
    Task<List<ArticleDto>> ExecuteAsync(DateTime from, DateTime to);
    Task<List<CategoryDto>> GetCategoriesAsync();
}
