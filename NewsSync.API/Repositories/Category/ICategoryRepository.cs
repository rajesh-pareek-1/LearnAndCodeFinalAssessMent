using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}
