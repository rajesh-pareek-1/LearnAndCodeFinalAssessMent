using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly NewsSyncNewsDbContext dbContext;
        private readonly ILogger<ReportService> logger;

        public ReportService(NewsSyncNewsDbContext dbContext, ILogger<ReportService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<bool> SubmitReportAsync(ReportDto dto)
        {
            try
            {
                var alreadyReported = await dbContext.ArticleReports
                    .AnyAsync(r => r.ArticleId == dto.ArticleId && r.ReportedByUserId == dto.UserId);

                if (alreadyReported)
                    throw new InvalidOperationException(ValidationMessages.DuplicateReport);

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
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "Duplicate report by user {UserId} for article {ArticleId}", dto.UserId, dto.ArticleId);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to submit report for article {ArticleId} by user {UserId}", dto.ArticleId, dto.UserId);
                throw new ApplicationException(ValidationMessages.FailedToSubmitReport, ex);
            }
        }

        private async Task BlockArticleIfThresholdReached(int articleId)
        {
            var reportCount = await dbContext.ArticleReports.CountAsync(r => r.ArticleId == articleId);

            if (reportCount >= 3)
            {
                var article = await dbContext.Articles.FindAsync(articleId);

                if (article is not null && !article.IsBlocked)
                {
                    article.IsBlocked = true;
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Article {ArticleId} blocked after {ReportCount} reports", articleId, reportCount);
                }
            }
        }
    }
}
