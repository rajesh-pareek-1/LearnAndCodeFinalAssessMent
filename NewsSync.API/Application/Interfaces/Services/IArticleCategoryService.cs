using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleCategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
