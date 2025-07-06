using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public CategoryRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await newsDb.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await newsDb.Categories.AddAsync(category);
        }

        public Task SaveChangesAsync()
        {
            return newsDb.SaveChangesAsync();
        }
    }
}
