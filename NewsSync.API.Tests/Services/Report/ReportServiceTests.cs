using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;
using Xunit;

namespace NewsSync.API.Tests.Services.Report
{
    public class ReportServiceTests
    {
        private readonly ReportService sut;
        private readonly NewsSyncNewsDbContext context;
        private readonly ILogger<ReportService> logger = new LoggerFactory().CreateLogger<ReportService>();

        public ReportServiceTests()
        {
            var options = new DbContextOptionsBuilder<NewsSyncNewsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new NewsSyncNewsDbContext(options);
            sut = new ReportService(context, logger);
        }

        [Fact]
        public async Task SubmitReportAsync_ShouldAddReport_WhenNotDuplicate()
        {
            // Arrange
            var dto = new ReportDto
            {
                ArticleId = 1,
                UserId = "user1",
                Reason = "Misleading content"
            };

            context.Articles.Add(new Article { Id = 1, Headline = "Fake", PublishedDate = DateTime.UtcNow.ToString() });
            await context.SaveChangesAsync();

            // Act
            var result = await sut.SubmitReportAsync(dto);

            // Assert
            result.Should().BeTrue();
            var reports = await context.ArticleReports.ToListAsync();
            reports.Should().ContainSingle(r => r.ArticleId == 1 && r.ReportedByUserId == "user1");
        }

        [Fact]
        public async Task SubmitReportAsync_ShouldThrow_WhenDuplicate()
        {
            // Arrange
            var articleId = 2;
            var userId = "user2";

            context.ArticleReports.Add(new ArticleReport { ArticleId = articleId, ReportedByUserId = userId });
            await context.SaveChangesAsync();

            var dto = new ReportDto
            {
                ArticleId = articleId,
                UserId = userId,
                Reason = "Spam"
            };

            // Act
            var act = async () => await sut.SubmitReportAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ValidationMessages.DuplicateReport);
        }

        [Fact]
        public async Task SubmitReportAsync_ShouldBlockArticle_WhenThresholdExceeded()
        {
            // Arrange
            var articleId = 3;
            var article = new Article { Id = articleId, Headline = "Block me", PublishedDate = DateTime.UtcNow.ToString(), IsBlocked = false };
            context.Articles.Add(article);

            for (int i = 0; i < 2; i++)
            {
                context.ArticleReports.Add(new ArticleReport { ArticleId = articleId, ReportedByUserId = $"user{i}" });
            }

            await context.SaveChangesAsync();

            var dto = new ReportDto
            {
                ArticleId = articleId,
                UserId = "user3",
                Reason = "Malicious content"
            };

            // Act
            var result = await sut.SubmitReportAsync(dto);

            // Assert
            result.Should().BeTrue();

            var updatedArticle = await context.Articles.FindAsync(articleId);
            updatedArticle.Should().NotBeNull();
            updatedArticle!.IsBlocked.Should().BeTrue();
        }
    }
}
