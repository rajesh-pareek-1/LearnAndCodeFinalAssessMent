using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public ArticleRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await newsDb.Articles
                .Where(a => !a.IsBlocked)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int articleId)
        {
            return await newsDb.Articles.FindAsync(articleId);
        }

        public async Task<List<ArticleReport>> GetReportsByArticleAsync(int articleId)
        {
            return await newsDb.ArticleReports
                .Where(r => r.ArticleId == articleId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddArticlesAsync(List<Article> articles)
        {
            await newsDb.Articles.AddRangeAsync(articles);
        }

        public async Task SubmitReportAsync(ReportDto reportDto)
        {
            if (await HasUserAlreadyReported(reportDto.ArticleId, reportDto.UserId))
                throw new InvalidOperationException("You have already reported this article.");

            await newsDb.ArticleReports.AddAsync(new ArticleReport
            {
                ArticleId = reportDto.ArticleId,
                ReportedByUserId = reportDto.UserId,
                Reason = reportDto.Reason
            });

            await EvaluateAutoBlockAsync(reportDto.ArticleId);

            await newsDb.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return newsDb.SaveChangesAsync();
        }

        private async Task<bool> HasUserAlreadyReported(int articleId, string userId)
        {
            return await newsDb.ArticleReports
                .AnyAsync(r => r.ArticleId == articleId && r.ReportedByUserId == userId);
        }

        private async Task EvaluateAutoBlockAsync(int articleId)
        {
            var reportCount = await newsDb.ArticleReports
                .CountAsync(r => r.ArticleId == articleId);

            if (reportCount >= 3)
            {
                var article = await newsDb.Articles.FindAsync(articleId);
                if (article is not null && !article.IsBlocked)
                {
                    article.IsBlocked = true;
                }
            }
        }
    }
}
