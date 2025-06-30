using System.Net.Http.Json;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Admin;
using NewsSyncClient.Core.Interfaces;

namespace NewsSyncClient.Application.Services;

public class AdminService : IAdminService
{
    private readonly HttpClient _client;


    public AdminService(IHttpClientProvider clientProvider)
    {
        _client = clientProvider.Client;
    }

    public async Task<List<ServerStatusDto>> GetServerStatusesAsync()
    {
        var resp = await _client.GetAsync("/api/admin/server");
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<ServerStatusDto>>() ?? new();
    }

    public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
    {
        var resp = await _client.GetAsync("/api/admin/server/details");
        if (!resp.IsSuccessStatusCode) return new();
        return await resp.Content.ReadFromJsonAsync<List<ServerDetailsDto>>() ?? [];
    }

    public async Task<bool> UpdateServerApiKeyAsync(int serverId, string newApiKey)
    {
        var payload = new { newApiKey };
        var resp = await _client.PutAsJsonAsync($"/api/admin/server/{serverId}", payload);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> AddCategoryAsync(string name, string description)
    {
        var payload = new { name, description };
        var resp = await _client.PostAsJsonAsync("/api/admin/category", payload);
        return resp.IsSuccessStatusCode;
    }
}
