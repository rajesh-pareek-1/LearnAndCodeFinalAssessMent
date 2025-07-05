using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Application.UseCases;

public class UpdateServerApiKeyUseCase : IUpdateServerApiKeyUseCase
{
    private readonly IAdminService _service;

    public UpdateServerApiKeyUseCase(IAdminService service)
    {
        _service = service;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var serverId = ConsoleInputHelper.ReadPositiveInt("Enter Server ID to update: ");
            var newApiKey = ConsoleInputHelper.ReadRequiredString("Enter new API Key: ");

            var success = await _service.UpdateServerApiKeyAsync(serverId, newApiKey);

            if (success)
                ConsoleOutputHelper.PrintSuccess("API key updated successfully.");
            else
                ConsoleOutputHelper.PrintError("Failed to update API key.");
        }
        catch (Exception ex)
        {
            ConsoleOutputHelper.PrintError($"Unexpected error: {ex.Message}");
        }
    }
}
