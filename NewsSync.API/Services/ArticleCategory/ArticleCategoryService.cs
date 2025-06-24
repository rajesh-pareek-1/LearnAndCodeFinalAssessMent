using NewsSync.API.Models.Domain;
using NewsSync.API.Repositories;

namespace NewsSync.API.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly ICategoryRepository repo;

        public ArticleCategoryService(ICategoryRepository repo)
        {
            this.repo = repo;
        }

        public Task<List<Category>> GetAllCategoriesAsync()
        {
            return repo.GetAllAsync();
        }
    }
}
