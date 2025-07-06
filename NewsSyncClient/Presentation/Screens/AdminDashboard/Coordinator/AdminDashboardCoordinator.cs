using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Screens;

public class AdminDashboardCoordinator : IAdminDashboardCoordinator
{
    private readonly IAdminRenderer _renderer;
    private readonly ISessionContext _session;
    private readonly IViewServerStatusUseCase _statusUseCase;
    private readonly IViewServerDetailsUseCase _detailsUseCase;
    private readonly IUpdateServerApiKeyUseCase _updateKeyUseCase;
    private readonly IAddCategoryUseCase _addCategoryUseCase;
    private readonly IErrorRenderer _errorRenderer;

    public AdminDashboardCoordinator(IAdminRenderer renderer, ISessionContext session, IViewServerStatusUseCase statusUseCase, IViewServerDetailsUseCase detailsUseCase, IUpdateServerApiKeyUseCase updateKeyUseCase, IAddCategoryUseCase addCategoryUseCase, IErrorRenderer errorRenderer)
    {
        _renderer = renderer;
        _session = session;
        _statusUseCase = statusUseCase;
        _detailsUseCase = detailsUseCase;
        _updateKeyUseCase = updateKeyUseCase;
        _addCategoryUseCase = addCategoryUseCase;
        _errorRenderer = errorRenderer;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            _renderer.RenderHeader(_session.Email);
            _renderer.RenderMenu();

            var choice = ConsoleInputHelper.ReadOptional("Select an option:")?.Trim();

            switch (choice)
            {
                case "1":
                    await ExecuteSafeAsync(() => _statusUseCase.ExecuteAsync());
                    break;

                case "2":
                    await ExecuteSafeAsync(() => _detailsUseCase.ExecuteAsync());
                    break;

                case "3":
                    await ExecuteSafeAsync(() => _updateKeyUseCase.ExecuteAsync());
                    break;

                case "4":
                    await ExecuteSafeAsync(() => _addCategoryUseCase.ExecuteAsync());
                    break;

                case "5":
                    _session.Clear();
                    ConsoleOutputHelper.PrintInfo("Logged out.");
                    return;

                default:
                    ConsoleOutputHelper.PrintWarning("Invalid option. Please select from 1 to 5.");
                    break;
            }

            ConsoleInputHelper.ReadOptional("\nPress Enter to continue...");
        }
    }

    private async Task ExecuteSafeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            _errorRenderer.Render(ex);
        }
    }
}
