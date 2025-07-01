using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Application.UseCases;

public class FetchNotificationsUseCase : IFetchNotificationsUseCase
{
    private readonly INotificationService _notificationService;

    public FetchNotificationsUseCase(INotificationService notificationService) =>
        _notificationService = notificationService;

    public Task<List<NotificationDto>> ExecuteAsync() =>
        _notificationService.GetNotificationsAsync();
}
