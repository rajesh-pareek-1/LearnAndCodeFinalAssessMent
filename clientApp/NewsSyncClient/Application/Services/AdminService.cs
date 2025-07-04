using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Admin;

namespace NewsSyncClient.Application.Services;

public class AdminService : IAdminService
{
    private readonly IApiClient _apiClient;

    public AdminService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<ServerStatusDto>> GetServerStatusesAsync()
    {
        return _apiClient.GetAsync<List<ServerStatusDto>>("/api/admin/server");
    }

    public Task<List<ServerDetailsDto>> GetServerDetailsAsync()
    {
        return _apiClient.GetAsync<List<ServerDetailsDto>>("/api/admin/server/details");
    }

    public Task<bool> UpdateServerApiKeyAsync(int serverId, string newApiKey)
    {
        if (serverId <= 0)
            throw new ValidationException("Server ID must be a positive number.");

        if (string.IsNullOrWhiteSpace(newApiKey))
            throw new ValidationException("API key cannot be empty.");

        var payload = new { newApiKey };
        return _apiClient.PutAsync($"/api/admin/server/{serverId}", payload);
    }

    public Task<bool> AddCategoryAsync(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Category name cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Category description cannot be empty.");

        var payload = new { name, description };
        return _apiClient.PostAsync("/api/admin/category", payload);
    }
}
