using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Notifications;

public class NotificationPreferencesUseCase : INotificationPreferencesUseCase
{
    private readonly INotificationService _notificationService;
    private readonly ISessionContext _sessionContext;

    public NotificationPreferencesUseCase(
        INotificationService notificationService,
        ISessionContext sessionContext)
    {
        _notificationService = notificationService;
        _sessionContext = sessionContext;
    }

    public async Task<List<NotificationCategoryDto>> GetCategoriesAsync()
    {
        var allCategories = await _notificationService.GetNotificationCategoriesAsync();

        var userId = _sessionContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidOperationException("User is not logged in.");

        var userPreferences = await _notificationService.GetUserNotificationPreferencesAsync(userId);

        var enabledCategoryIds = userPreferences
            .Select(p => p.CategoryId)
            .ToHashSet();

        return [.. allCategories
            .Select(category => new NotificationCategoryDto
            {
                Name = category.Name,
                IsEnabled = enabledCategoryIds.Contains(category.Id)
            })];
    }

    public Task<bool> UpdateCategoryPreferenceAsync(string categoryName, bool enabled) =>
        _notificationService.ConfigureNotificationAsync(categoryName, enabled);
}
