using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.Services;

public interface ISavedArticleService
{
    Task<List<ArticleDto>> GetSavedArticlesAsync();
    Task<bool> DeleteSavedArticleAsync(int articleId);
    Task SaveArticleAsync(int articleId);
}
