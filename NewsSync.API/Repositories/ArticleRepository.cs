using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsSyncNewsDbContext dbContext;

        public ArticleRepository(NewsSyncNewsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await dbContext.Articles.ToListAsync();
        }
    }
}
