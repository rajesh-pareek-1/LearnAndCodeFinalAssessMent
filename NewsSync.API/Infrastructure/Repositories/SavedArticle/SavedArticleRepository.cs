using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class SavedArticleRepository : ISavedArticleRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public SavedArticleRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task<List<Article>> GetSavedArticlesByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await newsDb.SavedArticles
                .Include(savedArticle => savedArticle.Article)
                .Where(savedArticle => savedArticle.UserId == userId)
                .Select(savedArticle => savedArticle.Article)
                .ToListAsync();
        }

        public async Task<bool> IsArticleAlreadySavedAsync(string userId, int articleId)
        {
            return await newsDb.SavedArticles
                .AnyAsync(savedArticle => savedArticle.UserId == userId && savedArticle.ArticleId == articleId);
        }

        public async Task<bool> DoesArticleExistAsync(int articleId)
        {
            return await newsDb.Articles.AnyAsync(article => article.Id == articleId);
        }

        public async Task SaveAsync(string userId, int articleId)
        {
            if (await IsArticleAlreadySavedAsync(userId, articleId))
                return;

            var savedArticle = new SavedArticle
            {
                UserId = userId,
                ArticleId = articleId
            };

            await newsDb.SavedArticles.AddAsync(savedArticle);
            await newsDb.SaveChangesAsync();
        }

        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            var savedArticle = await newsDb.SavedArticles
                .FirstOrDefaultAsync(savedArticle => savedArticle.UserId == userId && savedArticle.ArticleId == articleId);

            if (savedArticle == null)
                return false;

            newsDb.SavedArticles.Remove(savedArticle);
            await newsDb.SaveChangesAsync();

            return true;
        }
    }
}
