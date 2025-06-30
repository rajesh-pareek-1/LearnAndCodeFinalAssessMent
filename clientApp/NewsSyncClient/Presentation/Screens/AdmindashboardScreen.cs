using NewsSyncClient.Core.Interfaces.Screens;

namespace NewsSyncClient.Presentation.Screens;

public class AdminDashboardScreen : IAdminDashboardScreen
{
    private readonly IAdminDashboardCoordinator _coordinator;

    public AdminDashboardScreen(IAdminDashboardCoordinator coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task ShowAsync() => await _coordinator.StartAsync();
}
