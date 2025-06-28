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

        public async Task<List<ArticleResponseDto>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query)
        {
            var allArticles = await articleRepository.GetAllAsync();
            var filtered = FilterArticles(allArticles, fromDate, toDate, query);

            logger.LogInformation("Filtered {Count} articles with from: {From}, to: {To}, query: {Query}", filtered.Count, fromDate?.ToShortDateString(), toDate?.ToShortDateString(), query);

            return MapToDto(filtered);
        }

        public async Task<List<ArticleResponseDto>> SearchArticlesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return [];

            var allArticles = await articleRepository.GetAllAsync();

            var matched = allArticles
                .Where(a => a.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) || a.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            logger.LogInformation("Found {Count} articles matching query: {Query}", matched.Count, query);

            return MapToDto(matched);
        }

        public Task SubmitReportAsync(ReportDto dto)
        {
            return articleRepository.SubmitReportAsync(dto);
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
                filtered = articles
                    .Where(a => DateTime.TryParse(a.PublishedDate, out var publishedDate) && publishedDate.Date == today)
                    .ToList();
            }

            return filtered;
        }

        private static List<ArticleResponseDto> MapToDto(List<Article> articles)
        {
            return articles.Select(a => new ArticleResponseDto
            {
                Id = a.Id,
                Headline = a.Headline,
                Description = a.Description,
                Url = a.Url,
                PublishedDate = a.PublishedDate,
                CategoryId = a.CategoryId,
                IsBlocked = a.IsBlocked
            }).ToList();
        }
    }
}
