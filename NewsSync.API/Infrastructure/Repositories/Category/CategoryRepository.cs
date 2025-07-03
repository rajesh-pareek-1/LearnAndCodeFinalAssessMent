using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsSyncNewsDbContext db;

        public CategoryRepository(NewsSyncNewsDbContext db)
        {
            this.db = db;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await db.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await db.Categories.AddAsync(category);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
