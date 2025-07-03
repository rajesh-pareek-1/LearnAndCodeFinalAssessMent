namespace NewsSyncClient.Core.Interfaces.Prompts;

public interface INotificationsPrompt
{
    (string? categoryName, bool enabled) ReadConfigurationInput();
}
