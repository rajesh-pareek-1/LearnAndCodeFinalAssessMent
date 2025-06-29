using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly NewsSyncNewsDbContext dbContext;
        private readonly ILogger<ReportService> logger;

        private const int ReportThreshold = 3;

        public ReportService(NewsSyncNewsDbContext dbContext, ILogger<ReportService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<bool> SubmitReportAsync(ReportDto dto)
        {
            if (await IsDuplicateReport(dto.ArticleId, dto.UserId))
            {
                logger.LogWarning("Duplicate report by user {UserId} for article {ArticleId}", dto.UserId, dto.ArticleId);
                throw new InvalidOperationException(ValidationMessages.DuplicateReport);
            }

            try
            {
                var report = new ArticleReport
                {
                    ArticleId = dto.ArticleId,
                    ReportedByUserId = dto.UserId,
                    Reason = dto.Reason
                };

                dbContext.ArticleReports.Add(report);
                await dbContext.SaveChangesAsync();

                await BlockArticleIfThresholdReached(dto.ArticleId);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to submit report for article {ArticleId} by user {UserId}", dto.ArticleId, dto.UserId);
                throw new ApplicationException(ValidationMessages.FailedToSubmitReport, ex);
            }
        }

        private async Task<bool> IsDuplicateReport(int articleId, string userId)
        {
            return await dbContext.ArticleReports
                .AnyAsync(r => r.ArticleId == articleId && r.ReportedByUserId == userId);
        }

        private async Task BlockArticleIfThresholdReached(int articleId)
        {
            var reportCount = await dbContext.ArticleReports.CountAsync(r => r.ArticleId == articleId);

            if (reportCount < ReportThreshold)
                return;

            var article = await dbContext.Articles.FindAsync(articleId);
            if (article is null || article.IsBlocked)
                return;

            article.IsBlocked = true;
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Article {ArticleId} blocked after {ReportCount} reports", articleId, reportCount);
        }
    }
}
