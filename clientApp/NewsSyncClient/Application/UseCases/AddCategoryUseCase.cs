using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Application.UseCases;

public class AddCategoryUseCase : IAddCategoryUseCase
{
    private readonly IAdminService _service;

    public AddCategoryUseCase(IAdminService service)
    {
        _service = service;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            Console.Write("Enter category name: ");
            var name = Console.ReadLine()?.Trim();

            Console.Write("Enter description: ");
            var desc = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new UserInputException("Category name is required.");

            var success = await _service.AddCategoryAsync(name, desc ?? string.Empty);

            Console.WriteLine(success ? "Category added." : "Failed to add category.");
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"Input Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
