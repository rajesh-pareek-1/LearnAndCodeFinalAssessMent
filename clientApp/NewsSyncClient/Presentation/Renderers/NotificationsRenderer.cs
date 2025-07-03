using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Presentation.Renderers;

public class NotificationsRenderer : INotificationsRenderer
{
    public void RenderHeader() =>
        Console.WriteLine("=== Notifications ===\n");

    public void RenderNotifications(List<NotificationDto> notifications)
    {
        Console.Clear();
        Console.WriteLine("Recent Notifications:\n");

        foreach (var notification in notifications)
        {
            Console.WriteLine($"Notification ID : {notification.Id}");
            Console.WriteLine($"Headline        : {notification.Article.Headline}");
            Console.WriteLine($"Sent At         : {notification.SentAt:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine($"Author          : {notification.Article.AuthorName ?? "N/A"}");
            Console.WriteLine($"Source          : {notification.Article.Source ?? "Unknown"}");
            Console.WriteLine($"URL             : {notification.Article.Url}");
            Console.WriteLine(new string('-', 70));
        }
    }


    public void RenderCategories(List<NotificationCategoryDto> categories)
    {
        Console.WriteLine("Notification Categories:\n");
        foreach (var c in categories)
        {
            Console.WriteLine($"• {c.Name} – Enabled: {(c.IsEnabled ? "Yes" : "No")}");
        }
    }
}
