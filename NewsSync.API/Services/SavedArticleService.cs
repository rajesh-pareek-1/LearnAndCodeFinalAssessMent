using NewsSync.API.Models.Domain;
using NewsSync.API.Repositories;

namespace NewsSync.API.Services
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticleRepository repository;

        public SavedArticleService(ISavedArticleRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Article>> GetSavedArticlesForUserAsync(string userId)
        {
            return await repository.GetSavedArticlesByUserIdAsync(userId);
        }

        public async Task<bool> SaveArticleAsync(string userId, int articleId)
        {
            if (await repository.IsArticleAlreadySavedAsync(userId, articleId))
                return false;

            if (!await repository.DoesArticleExistAsync(articleId))
                return false;

            await repository.SaveAsync(userId, articleId);
            return true;
        }

        // Services/SavedArticleService.cs
        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            return await repository.DeleteSavedArticleAsync(userId, articleId);
        }

    }

}
