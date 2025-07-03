using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Application.UseCases;

public class UpdateServerApiKeyUseCase : IUpdateServerApiKeyUseCase
{
    private readonly IAdminService _service;

    public UpdateServerApiKeyUseCase(IAdminService service) { _service = service; }

    public async Task ExecuteAsync()
    {
        Console.Write("Enter Server ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid Server ID.");
            return;
        }

        Console.Write("Enter new API Key: ");
        var key = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(key))
        {
            Console.WriteLine("API key cannot be empty.");
            return;
        }

        var success = await _service.UpdateServerApiKeyAsync(id, key);
        Console.WriteLine(success ? "API key updated." : "Failed to update API key.");
    }
}
