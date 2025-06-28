using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;

namespace NewsSync.API.Application.Interfaces.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public async Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query)
        {
            var allArticles = await articleRepository.GetAllAsync();

            var filteredArticles = allArticles
                .Where(a =>
                    DateTime.TryParse(a.PublishedDate, out var publishedDate) &&
                    (
                        (!fromDate.HasValue || publishedDate >= fromDate.Value.Date) &&
                        (!toDate.HasValue || publishedDate <= toDate.Value.Date)
                    ) &&
                    (string.IsNullOrWhiteSpace(query) ||
                        a.Headline.Contains(query) || a.Description.Contains(query))
                )
                .ToList();

            if (!fromDate.HasValue && !toDate.HasValue && string.IsNullOrWhiteSpace(query))
            {
                var today = DateTime.UtcNow.Date;
                filteredArticles = allArticles
                    .Where(a =>
                        DateTime.TryParse(a.PublishedDate, out var publishedDate) &&
                        publishedDate.Date == today)
                    .ToList();
            }

            return filteredArticles;
        }
        public async Task<List<Article>> SearchArticlesAsync(string query)
        {
            var allArticles = await articleRepository.GetAllAsync();

            return allArticles
                .Where(a =>
                    !string.IsNullOrWhiteSpace(query) &&
                    (a.Headline.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     a.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task SubmitReportAsync(ReportDto dto)
        {
            await articleRepository.SubmitReportAsync(dto);
        }

    }

}
