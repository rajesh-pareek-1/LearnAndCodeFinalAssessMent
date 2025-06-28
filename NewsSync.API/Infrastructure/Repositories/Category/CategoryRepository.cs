using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsSyncNewsDbContext _newsDbContext;

        public CategoryRepository(NewsSyncNewsDbContext _newsDbContext)
        {
            this._newsDbContext = _newsDbContext;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _newsDbContext.Categories.ToListAsync();
        }
    }
}
