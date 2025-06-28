using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticleRepository repository;
        private readonly ILogger<SavedArticleService> logger;

        public SavedArticleService(ISavedArticleRepository repository, ILogger<SavedArticleService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<Article>> GetSavedArticlesForUserAsync(string userId)
        {
            try
            {
                return await repository.GetSavedArticlesByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch saved articles for user {UserId}", userId);
                throw new ApplicationException(ValidationMessages.FailedToFetchSavedArticles, ex);
            }
        }

        public async Task<bool> SaveArticleAsync(string userId, int articleId)
        {
            try
            {
                if (!await repository.DoesArticleExistAsync(articleId))
                {
                    logger.LogWarning("Attempted to save non-existent article {ArticleId} by user {UserId}", articleId, userId);
                    return false;
                }

                if (await repository.IsArticleAlreadySavedAsync(userId, articleId))
                {
                    logger.LogInformation("Article {ArticleId} is already saved by user {UserId}", articleId, userId);
                    return false;
                }

                await repository.SaveAsync(userId, articleId);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save article {ArticleId} for user {UserId}", articleId, userId);
                throw new ApplicationException(ValidationMessages.FailedToSaveArticle, ex);
            }
        }

        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            try
            {
                return await repository.DeleteSavedArticleAsync(userId, articleId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete saved article {ArticleId} for user {UserId}", articleId, userId);
                throw new ApplicationException(ValidationMessages.FailedToDeleteSavedArticle, ex);
            }
        }
    }
}
