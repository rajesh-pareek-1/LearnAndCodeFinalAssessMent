using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.Services;

namespace NewsSyncClient.Presentation.Screens;

public class AdminDashboardCoordinator : IAdminDashboardCoordinator
{
    private readonly IAdminService _adminService;
    private readonly IAdminRenderer _renderer;
    private readonly ISessionContext _session;

    public AdminDashboardCoordinator(IAdminService adminService, IAdminRenderer renderer, ISessionContext session)
    {
        _adminService = adminService;
        _renderer = renderer;
        _session = session;
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
                    await _renderer.RenderServerStatusesAsync(await _adminService.GetServerStatusesAsync());
                    break;
                case "2":
                    await _renderer.RenderServerDetailsAsync(await _adminService.GetServerDetailsAsync());
                    break;
                case "3":
                    await UpdateApiKeyAsync();
                    break;
                case "4":
                    await AddCategoryAsync();
                    break;
                case "5":
                    _session.Clear();
                    Console.WriteLine("Logged out successfully.");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task UpdateApiKeyAsync()
    {
        Console.Write("Enter Server ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid Server ID.");
            return;
        }

        Console.Write("Enter new API Key: ");
        var key = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(key))
        {
            Console.WriteLine("API key cannot be empty.");
            return;
        }

        var ok = await _adminService.UpdateServerApiKeyAsync(id, key);
        Console.WriteLine(ok ? "API key updated successfully." : "Failed to update API key.");
    }

    private async Task AddCategoryAsync()
    {
        Console.Write("Enter category name: ");
        var name = Console.ReadLine()?.Trim();
        Console.Write("Enter description: ");
        var desc = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Category name is required.");
            return;
        }

        var ok = await _adminService.AddCategoryAsync(name, desc);
        Console.WriteLine(ok ? "Category added successfully." : "Failed to add category.");
    }
}
