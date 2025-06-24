using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
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
