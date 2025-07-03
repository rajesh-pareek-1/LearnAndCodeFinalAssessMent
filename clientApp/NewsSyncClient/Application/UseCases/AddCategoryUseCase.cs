using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Application.UseCases;

public class AddCategoryUseCase : IAddCategoryUseCase
{
    private readonly IAdminService _service;

    public AddCategoryUseCase(IAdminService service) { _service = service; }

    public async Task ExecuteAsync()
    {
        Console.Write("Enter category name: ");
        var name = Console.ReadLine()?.Trim();

        Console.Write("Enter description: ");
        var desc = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Category name is required.");
            return;
        }

        var success = await _service.AddCategoryAsync(name, desc);
        Console.WriteLine(success ? "Category added." : "Failed to add category.");
    }
}
