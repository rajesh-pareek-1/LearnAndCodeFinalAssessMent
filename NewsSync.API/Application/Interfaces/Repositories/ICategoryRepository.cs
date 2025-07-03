using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task SaveChangesAsync();
    }
}
