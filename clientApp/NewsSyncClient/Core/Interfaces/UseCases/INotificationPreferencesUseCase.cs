using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Core.Interfaces.UseCases;

public interface INotificationPreferencesUseCase
{
    Task<List<NotificationCategoryDto>> GetCategoriesAsync();
    Task<bool> UpdateCategoryPreferenceAsync(string categoryName, bool enabled);
}
