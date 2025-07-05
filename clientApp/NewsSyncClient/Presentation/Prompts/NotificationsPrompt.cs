using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Prompts;

public class NotificationsPrompt : INotificationsPrompt
{
    public (string categoryName, bool enabled) ReadConfigurationInput()
    {
        var name = ConsoleInputHelper.ReadRequiredString("Enter category name: ");
        var enabled = ConsoleInputHelper.Confirm("Enable notifications for this category?");
        return (name, enabled);
    }
}
