using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

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
            var name = ConsoleInputHelper.ReadRequiredString("Enter category name: ");
            var desc = ConsoleInputHelper.ReadOptional("Enter description: ");

            var success = await _service.AddCategoryAsync(name, desc ?? string.Empty);

            if (success)
                ConsoleOutputHelper.PrintSuccess("Category added successfully.");
            else
                ConsoleOutputHelper.PrintError("Failed to add category.");
        }
        catch (UserInputException ex)
        {
            ConsoleOutputHelper.PrintWarning($"Input Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            ConsoleOutputHelper.PrintError($"Unexpected error: {ex.Message}");
        }
    }
}
