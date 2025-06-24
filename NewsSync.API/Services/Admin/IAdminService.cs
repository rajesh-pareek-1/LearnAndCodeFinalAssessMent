using NewsSync.API.Models.DTO;

namespace NewsSync.API.Services
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
