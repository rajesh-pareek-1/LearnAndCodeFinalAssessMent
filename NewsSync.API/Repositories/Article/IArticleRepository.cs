using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Repositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
        Task SubmitReportAsync(ReportDto dto);
         Task<List<ArticleReport>> GetReportsByArticleAsync(int articleId);

    }
}
