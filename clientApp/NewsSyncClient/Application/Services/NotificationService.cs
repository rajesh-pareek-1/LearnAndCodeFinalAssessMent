using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Categories;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IApiClient _apiClient;
    private readonly ISessionContext _session;

    public NotificationService(IApiClient apiClient, ISessionContext session)
    {
        _apiClient = apiClient;
        _session = session;
    }

    public Task<List<NotificationDto>> GetNotificationsAsync()
    {
        return _apiClient.GetAsync<List<NotificationDto>>($"/api/notification?userId={_session.UserId}");
    }

    public Task<List<CategoryDto>> GetNotificationCategoriesAsync()
    {
        return _apiClient.GetAsync<List<CategoryDto>>("/api/categories/article");
    }

    public Task<List<NotificationConfigDto>> GetUserNotificationPreferencesAsync(string userId)
    {
        return _apiClient.GetAsync<List<NotificationConfigDto>>($"/api/notification/configure?userId={userId}");
    }

    public Task<bool> ConfigureNotificationAsync(string categoryName, bool enabled)
    {
        var payload = new
        {
            userId = _session.UserId,
            categoryName,
            enabled
        };

        return _apiClient.PutAsync("/api/notification/configure", payload);
    }
}
