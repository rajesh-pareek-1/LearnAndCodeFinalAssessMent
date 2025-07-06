using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Core.Interfaces.UseCases;

public interface IFetchNotificationsUseCase
{
    Task<List<NotificationDto>> ExecuteAsync();
}
