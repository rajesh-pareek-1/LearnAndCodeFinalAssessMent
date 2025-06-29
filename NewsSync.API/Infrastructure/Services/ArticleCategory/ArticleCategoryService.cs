using AutoMapper;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;

namespace NewsSync.API.Application.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ArticleCategoryService> logger;

        public ArticleCategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<ArticleCategoryService> logger)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

            logger.LogInformation("Fetched {Count} categories from database.", categories.Count);

            return mapper.Map<List<CategoryResponseDto>>(categories);
        }
    }
}
