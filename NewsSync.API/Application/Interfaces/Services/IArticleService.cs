using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query);
        Task<List<Article>> SearchArticlesAsync(string query);
        Task SubmitReportAsync(ReportDto dto);
        
    }
}
