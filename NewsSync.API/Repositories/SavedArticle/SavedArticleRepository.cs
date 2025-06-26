using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public class SavedArticleRepository : ISavedArticleRepository
    {
        private readonly NewsSyncNewsDbContext _newsDbContext;

        public SavedArticleRepository(NewsSyncNewsDbContext _newsDbContext)
        {
            this._newsDbContext = _newsDbContext;
        }

        public async Task<List<Article>> GetSavedArticlesByUserIdAsync(string userId)
        {
            return await _newsDbContext.SavedArticles
                .Include(sa => sa.Article)
                .Where(sa => sa.UserId == userId)
                .Select(sa => sa.Article)
                .ToListAsync();
        }

        public async Task<bool> IsArticleAlreadySavedAsync(string userId, int articleId)
        {
            return await _newsDbContext.SavedArticles
                .AnyAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);
        }

        public async Task<bool> DoesArticleExistAsync(int articleId)
        {
            return await _newsDbContext.Articles.AnyAsync(a => a.Id == articleId);
        }

        public async Task SaveAsync(string userId, int articleId)
        {
            var savedArticle = new SavedArticle
            {
                UserId = userId,
                ArticleId = articleId
            };

            _newsDbContext.SavedArticles.Add(savedArticle);
            await _newsDbContext.SaveChangesAsync();

        }

        public async Task<bool> DeleteSavedArticleAsync(string userId, int articleId)
        {
            var savedArticle = await _newsDbContext.SavedArticles
                .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.ArticleId == articleId);

            if (savedArticle == null)
                return false;

            _newsDbContext.SavedArticles.Remove(savedArticle);
            await _newsDbContext.SaveChangesAsync();
            return true;
        }

    }
}
