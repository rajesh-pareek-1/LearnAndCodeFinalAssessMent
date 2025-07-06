using NewsSyncClient.Core.Models.Admin;

namespace NewsSyncClient.Core.Interfaces.Services;

public interface IAdminService
{
    Task<List<ServerStatusDto>> GetServerStatusesAsync();
    Task<List<ServerDetailsDto>> GetServerDetailsAsync();
    Task<bool> UpdateServerApiKeyAsync(int serverId, string newApiKey);
    Task<bool> AddCategoryAsync(string name, string description);
}
