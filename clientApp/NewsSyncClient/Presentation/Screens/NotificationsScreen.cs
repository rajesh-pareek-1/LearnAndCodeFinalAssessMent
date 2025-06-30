using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Presentation.Screens;

public class NotificationsScreen : INotificationsScreen
{
    private readonly ISessionContext _session;
    private readonly INotificationService _notificationService;

    public NotificationsScreen(ISessionContext session, INotificationService notificationService)
    {
        _session = session;
        _notificationService = notificationService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            DisplayHeader();

            Console.WriteLine("1. View Notifications");
            Console.WriteLine("2. Configure Notifications");
            Console.WriteLine("3. Back");
            Console.Write("\nEnter your choice: ");
            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    await ViewNotificationsAsync();
                    break;
                case "2":
                    await ConfigureNotificationsAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private void DisplayHeader()
    {
        Console.WriteLine($"Welcome {_session.Email}");
        Console.WriteLine($"Date: {DateTime.Now:dd-MMM-yyyy} | Time: {DateTime.Now:hh:mm tt}");
        Console.WriteLine("\n=== NOTIFICATIONS ===\n");
    }

    private async Task ViewNotificationsAsync()
    {
        var notifications = await _notificationService.GetNotificationsAsync();

        if (notifications.Count == 0)
        {
            Console.WriteLine("\nNo notifications found.");
            return;
        }

        Console.WriteLine("\nYour Notifications:\n");
        foreach (var note in notifications)
            DisplayNotification(note);
    }

    private void DisplayNotification(NotificationDto notification)
    {
        var article = notification.Article;
        Console.WriteLine($"Notification ID: {notification.Id}");
        Console.WriteLine($"Title: {article.Headline}");
        Console.WriteLine($"Published: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
        Console.WriteLine($"Author: {article.AuthorName ?? "N/A"}");
        Console.WriteLine($"Source: {article.Source}");
        Console.WriteLine($"URL: {article.Url}");
        Console.WriteLine($"Description: {article.Description}");
        Console.WriteLine($"Sent At: {notification.SentAt:dd-MMM-yyyy hh:mm tt}");
        Console.WriteLine(new string('-', 80));
    }

    private async Task ConfigureNotificationsAsync()
    {
        var categories = await _notificationService.GetNotificationCategoriesAsync();

        if (categories.Count == 0)
        {
            Console.WriteLine("\nNo categories available.");
            return;
        }

        Console.WriteLine("\nAvailable Categories:");
        foreach (var cat in categories)
            Console.WriteLine($"- {cat.Name} ({cat.Description})");

        Console.Write("\nEnter category to configure: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Category name is required.");
            return;
        }

        Console.Write("Enable notifications? (y/n): ");
        var enableInput = Console.ReadLine()?.Trim().ToLower();
        if (enableInput != "y" && enableInput != "n")
        {
            Console.WriteLine("Invalid input. Use 'y' or 'n'.");
            return;
        }

        var enabled = enableInput == "y";
        var success = await _notificationService.ConfigureNotificationAsync(name, enabled);

        Console.WriteLine(success ? "Notification configuration updated." : "Failed to update configuration.");
    }
}
