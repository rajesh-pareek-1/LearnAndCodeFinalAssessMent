using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Presentation.Screens;

public class AdminDashboardCoordinator : IAdminDashboardCoordinator
{
    private readonly IAdminRenderer _renderer;
    private readonly ISessionContext _session;
    private readonly IViewServerStatusUseCase _statusUseCase;
    private readonly IViewServerDetailsUseCase _detailsUseCase;
    private readonly IUpdateServerApiKeyUseCase _updateKeyUseCase;
    private readonly IAddCategoryUseCase _addCategoryUseCase;

    public AdminDashboardCoordinator(IAdminRenderer renderer, ISessionContext session, IViewServerStatusUseCase statusUseCase, IViewServerDetailsUseCase detailsUseCase, IUpdateServerApiKeyUseCase updateKeyUseCase, IAddCategoryUseCase addCategoryUseCase)
    {
        _renderer = renderer;
        _session = session;
        _statusUseCase = statusUseCase;
        _detailsUseCase = detailsUseCase;
        _updateKeyUseCase = updateKeyUseCase;
        _addCategoryUseCase = addCategoryUseCase;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            _renderer.RenderHeader(_session.Email);
            _renderer.RenderMenu();

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    await _statusUseCase.ExecuteAsync();
                    break;

                case "2":
                    await _detailsUseCase.ExecuteAsync();
                    break;

                case "3":
                    await _updateKeyUseCase.ExecuteAsync();
                    break;

                case "4":
                    await _addCategoryUseCase.ExecuteAsync();
                    break;

                case "5":
                    _session.Clear();
                    Console.WriteLine("Logged out.");
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
