using NewsSyncClient.Core.Exceptions;
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
        if (string.IsNullOrWhiteSpace(_session.UserId))
            throw new ValidationException("User must be logged in to fetch notifications.");

        return _apiClient.GetAsync<List<NotificationDto>>($"/api/notification?userId={_session.UserId}");
    }

    public Task<List<CategoryDto>> GetNotificationCategoriesAsync()
    {
        return _apiClient.GetAsync<List<CategoryDto>>("/api/categories/article");
    }

    public Task<List<NotificationConfigDto>> GetUserNotificationPreferencesAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ValidationException("User ID cannot be empty.");

        return _apiClient.GetAsync<List<NotificationConfigDto>>($"/api/notification/configure?userId={userId}");
    }

    public Task<bool> ConfigureNotificationAsync(string categoryName, bool enabled)
    {
        if (string.IsNullOrWhiteSpace(_session.UserId))
            throw new ValidationException("User must be logged in to configure notifications.");

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ValidationException("Category name cannot be empty.");

        var payload = new
        {
            userId = _session.UserId,
            categoryName,
            enabled
        };

        return _apiClient.PutAsync("/api/notification/configure", payload);
    }
}
