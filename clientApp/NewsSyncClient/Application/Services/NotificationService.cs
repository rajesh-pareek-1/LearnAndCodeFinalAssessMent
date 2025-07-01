using System.Net.Http.Json;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Categories;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Application.Services;

public class NotificationService : INotificationService
{
    private readonly HttpClient _client;
    private readonly ISessionContext _session;

    public NotificationService(IHttpClientProvider clientProvider, ISessionContext session)
    {
        _client = clientProvider.Client;
        _session = session;
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync()
    {
        var response = await _client.GetAsync($"/api/notification?userId={_session.UserId}");
        if (!response.IsSuccessStatusCode) return new();

        return await response.Content.ReadFromJsonAsync<List<NotificationDto>>() ?? new();
    }

    public async Task<List<CategoryDto>> GetNotificationCategoriesAsync()
    {
        var response = await _client.GetAsync("/api/categories/article");
        if (!response.IsSuccessStatusCode) return new();

        return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? [];
    }

    public async Task<List<NotificationConfigDto>> GetUserNotificationPreferencesAsync(string userId)
    {
        var response = await _client.GetAsync($"/api/notification/configure?userId={userId}");
        if (!response.IsSuccessStatusCode) return new();

        return await response.Content.ReadFromJsonAsync<List<NotificationConfigDto>>() ?? new();
    }


    public async Task<bool> ConfigureNotificationAsync(string categoryName, bool enabled)
    {
        var payload = new
        {
            userId = _session.UserId,
            categoryName,
            enabled
        };

        var response = await _client.PutAsJsonAsync("/api/notification/configure", payload);
        return response.IsSuccessStatusCode;
    }
}
