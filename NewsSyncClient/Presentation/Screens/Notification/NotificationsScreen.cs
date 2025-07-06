using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

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

            ConsoleOutputHelper.PrintInfo("1. View Notifications");
            ConsoleOutputHelper.PrintInfo("2. Configure Notifications");
            ConsoleOutputHelper.PrintInfo("3. Back");

            var choice = ConsoleInputHelper.ReadOptional("\nEnter your choice: ");
            if (choice == "3") return;

            if (menuActions.TryGetValue(choice ?? "", out var action))
                await action();
            else
                ConsoleOutputHelper.PrintError("Invalid option. Try again.");

            ConsoleInputHelper.ReadOptional("\nPress Enter to continue...");
        }
    }

    private async Task ShowNotificationsAsync()
    {
        var notifications = await _fetchUseCase.ExecuteAsync();

        if (!notifications.Any())
        {
            ConsoleOutputHelper.PrintWarning("No notifications found.");
            return;
        }

        _renderer.RenderNotifications(notifications);
    }

    private async Task ConfigurePreferencesAsync()
    {
        var categories = await _configureUseCase.GetCategoriesAsync();

        if (!categories.Any())
        {
            ConsoleOutputHelper.PrintWarning("No categories available.");
            return;
        }

        _renderer.RenderCategories(categories);

        var (categoryName, isEnabled) = _prompt.ReadConfigurationInput();
        if (string.IsNullOrWhiteSpace(categoryName)) return;

        var result = await _configureUseCase.UpdateCategoryPreferenceAsync(categoryName, isEnabled);

        if (result)
            ConsoleOutputHelper.PrintSuccess("Notification preferences updated.");
        else
            ConsoleOutputHelper.PrintError("Failed to update preferences.");
    }
}
