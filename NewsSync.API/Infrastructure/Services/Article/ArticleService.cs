using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IUserPreferenceRepository preferenceRepository;
        private readonly ILogger<ArticleService> logger;

        public ArticleService(IArticleRepository articleRepository, IUserPreferenceRepository preferenceRepository, ILogger<ArticleService> logger)
        {
            this.articleRepository = articleRepository;
            this.preferenceRepository = preferenceRepository;
            this.logger = logger;
        }

        public async Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query, string? userId)
        {
            var allArticles = await articleRepository.GetAllAsync();
            var filtered = FilterArticles(allArticles, fromDate, toDate, query);

            if (!string.IsNullOrWhiteSpace(userId))
                filtered = await SortByUserPreferenceAsync(filtered, userId);

            var topArticles = filtered.Take(50).ToList();

            logger.LogInformation("Filtered {Count} articles with from: {From}, to: {To}, query: {Query}", topArticles.Count, fromDate?.ToShortDateString(), toDate?.ToShortDateString(), query);

            return topArticles;
        }

        public async Task<List<Article>> SearchArticlesAsync(string query, string? userId)
        {
            if (string.IsNullOrWhiteSpace(query)) return [];

            var allArticles = await articleRepository.GetAllAsync();

            var matched = allArticles
                .Where(a => a.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) || a.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!string.IsNullOrWhiteSpace(userId))
                matched = await SortByUserPreferenceAsync(matched, userId);

            logger.LogInformation("Found {Count} articles matching query: {Query}", matched.Count, query);
            return matched;
        }

        public Task SubmitReportAsync(ReportDto reportDto) => articleRepository.SubmitReportAsync(reportDto);

        private static List<Article> FilterArticles(List<Article> articles, DateTime? fromDate, DateTime? toDate, string? query)
        {
            query = string.IsNullOrWhiteSpace(query) ? null : query.Trim();

            var dateFiltered = articles
                .Where(article =>
                    DateTime.TryParse(article.PublishedDate, out var publishedDate) &&
                    (!fromDate.HasValue || publishedDate >= fromDate.Value.Date) &&
                    (!toDate.HasValue || publishedDate < toDate.Value.Date.AddDays(1)))
                .ToList();

            var finalFiltered = string.IsNullOrWhiteSpace(query)
                ? dateFiltered
                : [.. dateFiltered
                    .Where(article =>
                        article.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        article.Description.Contains(query, StringComparison.OrdinalIgnoreCase))];

            if (!fromDate.HasValue && !toDate.HasValue && string.IsNullOrWhiteSpace(query))
            {
                var today = DateTime.UtcNow.Date;
                finalFiltered = [.. articles.Where(article => DateTime.TryParse(article.PublishedDate, out var publishedDate) && publishedDate.Date == today)];
            }

            return finalFiltered;
        }

        public async Task<List<Article>> SortByUserPreferenceAsync(List<Article> articles, string? userId)
        {
            var preferredCategoryIds = await preferenceRepository.GetPreferredCategoryIdsAsync(userId);

            return [.. articles
                .OrderByDescending(a => preferredCategoryIds.Contains(a.CategoryId) ? 1 : 0)
                .ThenByDescending(a => DateTime.TryParse(a.PublishedDate, out var dt) ? dt : DateTime.MinValue)];
        }
    }
}
