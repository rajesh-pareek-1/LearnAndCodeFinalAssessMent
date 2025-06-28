using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;

namespace NewsSync.API.Application.Interfaces.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository repo;

        public AdminService(IAdminRepository repo)
        {
            this.repo = repo;
        }

        public async Task AddCategoryAsync(CategoryCreateRequestDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
            await repo.AddCategoryAsync(category);
            await repo.SaveChangesAsync();
        }

        public Task<List<ServerStatusDto>> GetServerStatusAsync()
            => repo.GetServerStatusAsync();

        public Task<List<ServerDetailsDto>> GetServerDetailsAsync()
            => repo.GetServerDetailsAsync();

        public async Task UpdateServerApiKeyAsync(int serverId, string newApiKey)
        {
            var server = await repo.GetServerByIdAsync(serverId);
            if (server == null) throw new Exception("Server not found");
            server.ApiKey = newApiKey;
            await repo.SaveChangesAsync();
        }

        public async Task BlockArticleAsync(int articleId, bool block)
        {
            var article = await repo.GetArticleByIdAsync(articleId);
            if (article == null) throw new Exception("Article not found");

            article.IsBlocked = block;
            await repo.SaveChangesAsync();
        }

    }
}
