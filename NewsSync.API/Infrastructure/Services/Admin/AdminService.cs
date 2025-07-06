using AutoMapper;
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
        private readonly IMapper mapper;

        public AdminService(ICategoryRepository categoryRepository, IServerRepository serverRepository, IArticleRepository articleRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.serverRepository = serverRepository;
            this.articleRepository = articleRepository;
            this.mapper = mapper;
        }

        public async Task AddCategoryAsync(CategoryCreateRequestDto categoryCreateRequestDto)
        {
            var category = mapper.Map<Category>(categoryCreateRequestDto);

            await categoryRepository.AddAsync(category);
            await categoryRepository.SaveChangesAsync();
        }

        public Task<List<ServerStatusDto>> GetServerStatusAsync()
            => serverRepository.GetServerStatusAsync();

        public Task<List<ServerDetailsDto>> GetServerDetailsAsync()
            => serverRepository.GetServerDetailsAsync();

        public async Task UpdateServerApiKeyAsync(int serverId, string newApiKey)
        {
            var server = await serverRepository.GetByIdAsync(serverId) ?? throw new Exception("Server not found");
            server.ApiKey = newApiKey;
            await serverRepository.SaveChangesAsync();
        }

        public async Task BlockArticleAsync(int articleId, bool block)
        {
            var article = await articleRepository.GetByIdAsync(articleId) ?? throw new Exception("Article not found");
            article.IsBlocked = block;
            await articleRepository.SaveChangesAsync();
        }
    }
}
