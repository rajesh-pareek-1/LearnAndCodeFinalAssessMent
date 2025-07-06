using NewsSync.API.Application.DTOs;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query, string? userId);
        Task<List<Article>> SearchArticlesAsync(string query, string? userId);
        Task SubmitReportAsync(ReportDto dto);
        Task<List<Article>> SortByUserPreferenceAsync(List<Article> articles, string? userId);
    }
}
