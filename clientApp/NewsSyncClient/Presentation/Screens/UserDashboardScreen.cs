using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;

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

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Headlines");
            Console.WriteLine("2. Saved Articles");
            Console.WriteLine("3. Search Articles");
            Console.WriteLine("4. Notifications");
            Console.WriteLine("5. Logout");
            Console.Write("\nEnter your choice: ");

            var input = Console.ReadLine()?.Trim();

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
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress Enter to return to the dashboard...");
                Console.ReadLine();
            }
        }
    }

    private void RenderHeader()
    {
        Console.WriteLine($"Welcome, {_session.Email}");
        Console.WriteLine($"Date: {DateTime.Now:dd-MMM-yyyy}");
        Console.WriteLine($"Time: {DateTime.Now:hh:mm tt}\n");
    }

    private void PerformLogout()
    {
        _session.Clear();
        Console.WriteLine("You have been logged out.");
    }
}
