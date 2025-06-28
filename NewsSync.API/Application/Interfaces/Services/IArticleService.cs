using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleService
    {
        Task<List<ArticleResponseDto>> GetFilteredArticlesAsync(DateTime? fromDate, DateTime? toDate, string? query);
        Task<List<ArticleResponseDto>> SearchArticlesAsync(string query);
        Task SubmitReportAsync(ReportDto dto);
    }
}
