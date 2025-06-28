using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsSyncNewsDbContext _newsDbContext;

        public ArticleRepository(NewsSyncNewsDbContext _newsDbContext)
        {
            this._newsDbContext = _newsDbContext;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _newsDbContext.Articles.Where(a => !a.IsBlocked).ToListAsync();
        }

        public async Task SubmitReportAsync(ReportDto dto)
        {
            var alreadyReported = await _newsDbContext.ArticleReports
                .AnyAsync(r => r.ArticleId == dto.ArticleId && r.ReportedByUserId == dto.UserId);

            if (alreadyReported)
                throw new Exception("You have already reported this article.");

            var report = new ArticleReport
            {
                ArticleId = dto.ArticleId,
                ReportedByUserId = dto.UserId,
                Reason = dto.Reason
            };

            await _newsDbContext.ArticleReports.AddAsync(report);

            var totalReports = await _newsDbContext.ArticleReports
                .CountAsync(r => r.ArticleId == dto.ArticleId);

            if (totalReports + 1 >= 3)
            {
                var article = await _newsDbContext.Articles.FindAsync(dto.ArticleId);
                if (article != null && !article.IsBlocked)
                {
                    article.IsBlocked = true;
                }
            }

            await _newsDbContext.SaveChangesAsync();
        }

        public async Task<List<ArticleReport>> GetReportsByArticleAsync(int articleId)
        {
            return await _newsDbContext.ArticleReports
                .Where(r => r.ArticleId == articleId)
                .ToListAsync();
        }

    }
}
