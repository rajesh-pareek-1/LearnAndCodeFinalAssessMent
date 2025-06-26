using NewsSync.API.Models.Domain;

namespace NewsSync.API.Services
{
    public interface IArticleCategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
