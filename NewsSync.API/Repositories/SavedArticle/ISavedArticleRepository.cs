using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public interface ISavedArticleRepository
    {
        Task<List<Article>> GetSavedArticlesByUserIdAsync(string userId);
        Task<bool> DoesArticleExistAsync(int articleId);
        Task<bool> IsArticleAlreadySavedAsync(string userId, int articleId);
        Task SaveAsync(string userId, int articleId);
        Task<bool> DeleteSavedArticleAsync(string userId, int articleId);
    }
}
