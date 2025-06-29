using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly ILogger<ArticleService> logger;

        public ArticleService(IArticleRepository articleRepository, ILogger<ArticleService> logger)
        {
            this.articleRepository = articleRepository;
            this.logger = logger;
        }

        public async Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query)
        {
            var allArticles = await articleRepository.GetAllAsync();
            var filtered = FilterArticles(allArticles, fromDate, toDate, query);

            logger.LogInformation("Filtered {Count} articles with from: {From}, to: {To}, query: {Query}", filtered.Count, fromDate?.ToShortDateString(), toDate?.ToShortDateString(), query);

            return filtered;
        }

        public async Task<List<Article>> SearchArticlesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return [];

            var allArticles = await articleRepository.GetAllAsync();

            var matched = allArticles
                .Where(a => a.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) || a.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            logger.LogInformation("Found {Count} articles matching query: {Query}", matched.Count, query);

            return matched;
        }

        public Task SubmitReportAsync(ReportDto reportDto)
        {
            return articleRepository.SubmitReportAsync(reportDto);
        }

        private static List<Article> FilterArticles(List<Article> articles, DateTime? fromDate, DateTime? toDate, string? query)
        {
            var filtered = articles
                .Where(a =>
                    DateTime.TryParse(a.PublishedDate, out var publishedDate) &&
                    (!fromDate.HasValue || publishedDate >= fromDate.Value.Date) &&
                    (!toDate.HasValue || publishedDate <= toDate.Value.Date) &&
                    (string.IsNullOrWhiteSpace(query) || a.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) || a.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!fromDate.HasValue && !toDate.HasValue && string.IsNullOrWhiteSpace(query))
            {
                var today = DateTime.UtcNow.Date;
                filtered = [.. articles.Where(a => DateTime.TryParse(a.PublishedDate, out var publishedDate) && publishedDate.Date == today)];
            }

            return filtered;
        }
    }
}
