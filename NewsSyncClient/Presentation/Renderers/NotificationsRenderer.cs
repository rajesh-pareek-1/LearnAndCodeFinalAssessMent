using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Notifications;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Renderers;

public class NotificationsRenderer : INotificationsRenderer
{
    public void RenderHeader() =>
        ConsoleOutputHelper.PrintHeader("Notifications");

    public void RenderNotifications(List<NotificationDto> notifications)
    {
        Console.Clear();
        ConsoleOutputHelper.PrintHeader("Recent Notifications");

        foreach (var notification in notifications)
        {
            ConsoleOutputHelper.PrintInfo($"Notification ID : {notification.Id}");
            ConsoleOutputHelper.PrintInfo($"Headline        : {notification.Article.Headline}");
            ConsoleOutputHelper.PrintInfo($"Sent At         : {notification.SentAt:dd-MMM-yyyy hh:mm tt}");
            ConsoleOutputHelper.PrintInfo($"Author          : {notification.Article.AuthorName ?? "N/A"}");
            ConsoleOutputHelper.PrintInfo($"Source          : {notification.Article.Source ?? "Unknown"}");
            ConsoleOutputHelper.PrintInfo($"URL             : {notification.Article.Url}");
            ConsoleOutputHelper.PrintDivider();
        }
    }

    public void RenderCategories(List<NotificationCategoryDto> categories)
    {
        ConsoleOutputHelper.PrintHeader("Notification Categories");

        foreach (var c in categories)
        {
            ConsoleOutputHelper.PrintInfo($"• {c.Name} – Enabled: {(c.IsEnabled ? "Yes" : "No")}");
        }
    }
}
