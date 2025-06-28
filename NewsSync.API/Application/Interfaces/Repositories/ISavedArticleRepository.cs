using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
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
