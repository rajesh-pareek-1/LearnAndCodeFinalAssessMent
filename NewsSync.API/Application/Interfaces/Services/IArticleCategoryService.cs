using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleCategoryService
    {
        Task<List<CategoryResponseDto>> GetAllCategoriesAsync();
    }
}
