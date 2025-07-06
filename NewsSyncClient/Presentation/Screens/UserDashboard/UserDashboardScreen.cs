using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Screens;

public class UserDashboardScreen
{
    private readonly IHeadlinesScreen _headlinesScreen;
    private readonly ISavedArticlesScreen _savedArticlesScreen;
    private readonly ISearchArticlesScreen _searchArticlesScreen;
    private readonly INotificationsScreen _notificationsScreen;
    private readonly ISessionContext _session;

    public UserDashboardScreen(IHeadlinesScreen headlinesScreen, ISavedArticlesScreen savedArticlesScreen, ISearchArticlesScreen searchArticlesScreen, INotificationsScreen notificationsScreen, ISessionContext session)
    {
        _headlinesScreen = headlinesScreen;
        _savedArticlesScreen = savedArticlesScreen;
        _searchArticlesScreen = searchArticlesScreen;
        _notificationsScreen = notificationsScreen;
        _session = session;
    }

    public async Task ShowAsync()
    {
        var exit = false;

        while (!exit)
        {
            Console.Clear();
            RenderHeader();

            ConsoleOutputHelper.PrintInfo("Please choose an option:");
            ConsoleOutputHelper.PrintInfo("1. Headlines");
            ConsoleOutputHelper.PrintInfo("2. Saved Articles");
            ConsoleOutputHelper.PrintInfo("3. Search Articles");
            ConsoleOutputHelper.PrintInfo("4. Notifications");
            ConsoleOutputHelper.PrintInfo("5. Logout");

            var input = ConsoleInputHelper.ReadOptional("\nEnter your choice: ")?.Trim();

            switch (input)
            {
                case "1":
                    await _headlinesScreen.ShowAsync();
                    break;
                case "2":
                    await _savedArticlesScreen.ShowAsync();
                    break;
                case "3":
                    await _searchArticlesScreen.ShowAsync();
                    break;
                case "4":
                    await _notificationsScreen.ShowAsync();
                    break;
                case "5":
                    PerformLogout();
                    exit = true;
                    break;
                default:
                    ConsoleOutputHelper.PrintWarning("Invalid input. Try again.");
                    break;
            }

            if (!exit)
                ConsoleInputHelper.ReadOptional("\nPress Enter to return to the dashboard...");
        }
    }

    private void RenderHeader()
    {
        ConsoleOutputHelper.PrintHeader($"Welcome, {_session.Email}");
        ConsoleOutputHelper.PrintInfo($"Date: {DateTime.Now:dd-MMM-yyyy}");
        ConsoleOutputHelper.PrintInfo($"Time: {DateTime.Now:hh:mm tt}\n");
    }

    private void PerformLogout()
    {
        _session.Clear();
        ConsoleOutputHelper.PrintSuccess("You have been logged out.");
    }
}
