using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
        Task SubmitReportAsync(ReportDto dto);
        Task<List<ArticleReport>> GetReportsByArticleAsync(int articleId);
        Task AddArticlesAsync(List<Article> articles);
        Task SaveChangesAsync();
        Task<Article?> GetByIdAsync(int articleId);
    }
}
