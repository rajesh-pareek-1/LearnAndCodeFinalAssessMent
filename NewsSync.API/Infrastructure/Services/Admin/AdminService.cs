using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IServerRepository serverRepository;
        private readonly IArticleRepository articleRepository;

        public AdminService(ICategoryRepository categoryRepository, IServerRepository serverRepository, IArticleRepository articleRepository)
        {
            this.categoryRepository = categoryRepository;
            this.serverRepository = serverRepository;
            this.articleRepository = articleRepository;
        }

        public async Task AddCategoryAsync(CategoryCreateRequestDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await categoryRepository.AddAsync(category);
            await categoryRepository.SaveChangesAsync();
        }

        public Task<List<ServerStatusDto>> GetServerStatusAsync()
            => serverRepository.GetServerStatusAsync();

        public Task<List<ServerDetailsDto>> GetServerDetailsAsync()
            => serverRepository.GetServerDetailsAsync();

        public async Task UpdateServerApiKeyAsync(int serverId, string newApiKey)
        {
            var server = await serverRepository.GetByIdAsync(serverId);
            if (server == null)
                throw new Exception("Server not found");

            server.ApiKey = newApiKey;
            await serverRepository.SaveChangesAsync();
        }

        public async Task BlockArticleAsync(int articleId, bool block)
        {
            var article = await articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new Exception("Article not found");

            article.IsBlocked = block;
            await articleRepository.SaveChangesAsync();
        }
    }
}
