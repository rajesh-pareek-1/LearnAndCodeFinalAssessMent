using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

public class NotificationsScreen : INotificationsScreen
{
    private readonly IFetchNotificationsUseCase _fetchUseCase;
    private readonly INotificationPreferencesUseCase _configureUseCase;
    private readonly INotificationsRenderer _renderer;
    private readonly INotificationsPrompt _prompt;

    public NotificationsScreen(IFetchNotificationsUseCase fetchUseCase, INotificationPreferencesUseCase configureUseCase, INotificationsRenderer renderer, INotificationsPrompt prompt)
    {
        _fetchUseCase = fetchUseCase;
        _configureUseCase = configureUseCase;
        _renderer = renderer;
        _prompt = prompt;
    }

    public async Task ShowAsync()
    {
        var menuActions = new Dictionary<string, Func<Task>>
        {
            ["1"] = ShowNotificationsAsync,
            ["2"] = ConfigurePreferencesAsync,
            ["3"] = () => Task.CompletedTask
        };

        while (true)
        {
            _renderer.RenderHeader();

            Console.WriteLine("1. View Notifications");
            Console.WriteLine("2. Configure Notifications");
            Console.WriteLine("3. Back");
            Console.Write("\nEnter your choice: ");
            var choice = Console.ReadLine()?.Trim();

            if (choice == "3") return;

            if (menuActions.TryGetValue(choice ?? "", out var action))
                await action();
            else
                Console.WriteLine("Invalid option. Try again.");

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task ShowNotificationsAsync()
    {
        var notifications = await _fetchUseCase.ExecuteAsync();

        if (!notifications.Any())
        {
            Console.WriteLine("\nNo notifications found.");
            return;
        }

        _renderer.RenderNotifications(notifications);
    }

    private async Task ConfigurePreferencesAsync()
    {
        var categories = await _configureUseCase.GetCategoriesAsync();

        if (!categories.Any())
        {
            Console.WriteLine("\nNo categories available.");
            return;
        }

        _renderer.RenderCategories(categories);

        var (categoryName, isEnabled) = _prompt.ReadConfigurationInput();
        if (string.IsNullOrWhiteSpace(categoryName)) return;

        var result = await _configureUseCase.UpdateCategoryPreferenceAsync(categoryName, isEnabled);
        Console.WriteLine(result ? "Notification preferences updated." : "Failed to update preferences.");
    }
}
