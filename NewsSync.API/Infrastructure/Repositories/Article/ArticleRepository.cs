using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsSyncNewsDbContext db;

        public ArticleRepository(NewsSyncNewsDbContext db)
        {
            this.db = db;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await db.Articles
                .Where(a => !a.IsBlocked)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int articleId)
        {
            return await db.Articles.FindAsync(articleId);
        }

        public async Task<List<ArticleReport>> GetReportsByArticleAsync(int articleId)
        {
            return await db.ArticleReports
                .Where(r => r.ArticleId == articleId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddArticlesAsync(List<Article> articles)
        {
            await db.Articles.AddRangeAsync(articles);
        }

        public async Task SubmitReportAsync(ReportDto dto)
        {
            if (await HasUserAlreadyReported(dto.ArticleId, dto.UserId))
                throw new InvalidOperationException("You have already reported this article.");

            await db.ArticleReports.AddAsync(new ArticleReport
            {
                ArticleId = dto.ArticleId,
                ReportedByUserId = dto.UserId,
                Reason = dto.Reason
            });

            await EvaluateAutoBlockAsync(dto.ArticleId);

            await db.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        private async Task<bool> HasUserAlreadyReported(int articleId, string userId)
        {
            return await db.ArticleReports
                .AnyAsync(r => r.ArticleId == articleId && r.ReportedByUserId == userId);
        }

        private async Task EvaluateAutoBlockAsync(int articleId)
        {
            var reportCount = await db.ArticleReports
                .CountAsync(r => r.ArticleId == articleId);

            if (reportCount >= 3)
            {
                var article = await db.Articles.FindAsync(articleId);
                if (article is not null && !article.IsBlocked)
                {
                    article.IsBlocked = true;
                }
            }
        }
    }
}
