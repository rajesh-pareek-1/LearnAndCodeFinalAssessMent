using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;

namespace NewsSyncClient.Presentation.Prompts;

public class NotificationsPrompt : INotificationsPrompt
{
    public (string categoryName, bool enabled) ReadConfigurationInput()
    {
        Console.Write("\nEnter category name: ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new UserInputException("Category name cannot be empty.");

        Console.Write("Enable notifications for this category? (y/n): ");
        var response = Console.ReadLine()?.Trim().ToLower();

        if (response != "y" && response != "n")
            throw new UserInputException("Please enter 'y' for yes or 'n' for no.");

        var enabled = response == "y";
        return (name, enabled);
    }
}
