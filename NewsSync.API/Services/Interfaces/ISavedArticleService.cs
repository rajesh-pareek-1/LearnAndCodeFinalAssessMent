using NewsSync.API.Models.Domain;

namespace NewsSync.API.Services
{
    public interface ISavedArticleService
    {
        Task<List<Article>> GetSavedArticlesForUserAsync(string userId);
        Task<bool> SaveArticleAsync(string userId, int articleId);
        Task<bool> DeleteSavedArticleAsync(string userId, int articleId);

    }
}
