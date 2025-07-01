using NewsSyncClient.Core.Interfaces.Prompts;

namespace NewsSyncClient.Presentation.Prompts;

public class NotificationsPrompt : INotificationsPrompt
{
    public (string categoryName, bool enabled) ReadConfigurationInput()
    {
        Console.Write("\nEnter category name: ");
        var name = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Enable notifications for this category? (y/n): ");
        var enabled = (Console.ReadLine()?.Trim().ToLower() == "y");
        
        return (name, enabled);
    }
}
