using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;

namespace NewsSync.API.Application.Interfaces.Services
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
