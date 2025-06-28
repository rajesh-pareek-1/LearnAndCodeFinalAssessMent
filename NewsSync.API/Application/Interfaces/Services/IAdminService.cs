using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task AddCategoryAsync(CategoryCreateRequestDto dto);
        Task<List<ServerStatusDto>> GetServerStatusAsync();
        Task<List<ServerDetailsDto>> GetServerDetailsAsync();
        Task UpdateServerApiKeyAsync(int serverId, string newApiKey);
        Task BlockArticleAsync(int articleId, bool block);

    }
}
