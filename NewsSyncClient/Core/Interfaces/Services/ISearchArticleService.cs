using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.Services;

public interface ISearchArticleService
{
    Task<List<ArticleDto>> SearchAsync(string query);
}
