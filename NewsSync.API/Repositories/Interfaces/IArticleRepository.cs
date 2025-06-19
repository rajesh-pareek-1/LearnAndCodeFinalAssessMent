using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
    }
}
