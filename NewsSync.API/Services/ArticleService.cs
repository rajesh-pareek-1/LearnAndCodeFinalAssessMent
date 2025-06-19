using NewsSync.API.Models.Domain;
using NewsSync.API.Repositories;

namespace NewsSync.API.Services
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

            // Default case — no filters
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

    }

}
