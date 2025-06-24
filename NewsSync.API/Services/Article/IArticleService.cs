using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query);
        Task<List<Article>> SearchArticlesAsync(string query);
        Task SubmitReportAsync(ReportDto dto);
        
    }
}
