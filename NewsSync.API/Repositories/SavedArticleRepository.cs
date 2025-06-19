using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public class SavedArticleRepository : ISavedArticleRepository
    {
        private readonly NewsSyncNewsDbContext dbContext;

        public SavedArticleRepository(NewsSyncNewsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Article>> GetSavedArticlesByUserIdAsync(string userId)
        {
            return await dbContext.SavedArticles
                .Include(sa => sa.Article)
                .Where(sa => sa.UserId == userId)
                .Select(sa => sa.Article)
                .ToListAsync();
        }

        public async Task<bool> IsArticleAlreadySavedAsync(string userId, int articleId)
        {
            return await dbContext.SavedArticles
                .AnyAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);
        }

        public async Task<bool> DoesArticleExistAsync(int articleId)
        {
            return await dbContext.Articles.AnyAsync(a => a.Id == articleId);
        }

        public async Task SaveAsync(string userId, int articleId)
        {
            var savedArticle = new SavedArticle
            {
                UserId = userId,
                ArticleId = articleId
            };

            dbContext.SavedArticles.Add(savedArticle);
            await dbContext.SaveChangesAsync();

        }

        // Repositories/SavedArticleRepository.cs
        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            var savedArticle = await dbContext.SavedArticles
                .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);

            if (savedArticle == null)
                return false;

            dbContext.SavedArticles.Remove(savedArticle);
            await dbContext.SaveChangesAsync();
            return true;
        }

    }
}
