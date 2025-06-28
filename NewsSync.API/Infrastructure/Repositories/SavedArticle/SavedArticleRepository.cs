using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class SavedArticleRepository : ISavedArticleRepository
    {
        private readonly NewsSyncNewsDbContext db;

        public SavedArticleRepository(NewsSyncNewsDbContext db)
        {
            this.db = db;
        }

        public async Task<List<Article>> GetSavedArticlesByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await db.SavedArticles
                .Include(sa => sa.Article)
                .Where(sa => sa.UserId == userId)
                .Select(sa => sa.Article)
                .ToListAsync();
        }

        public async Task<bool> IsArticleAlreadySavedAsync(string userId, int articleId)
        {
            return await db.SavedArticles
                .AnyAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);
        }

        public async Task<bool> DoesArticleExistAsync(int articleId)
        {
            return await db.Articles.AnyAsync(a => a.Id == articleId);
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

            await db.SavedArticles.AddAsync(savedArticle);
            await db.SaveChangesAsync();
        }

        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            var savedArticle = await db.SavedArticles
                .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);

            if (savedArticle == null)
                return false;

            db.SavedArticles.Remove(savedArticle);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
