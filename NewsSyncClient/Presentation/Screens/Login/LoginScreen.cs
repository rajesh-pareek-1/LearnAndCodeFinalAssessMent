using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Screens;

public class LoginScreen
{
    private readonly IAuthService _authService;

    public LoginScreen(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> ShowAsync()
    {
        Console.Clear();
        ConsoleOutputHelper.PrintHeader("Login");

        var username = ConsoleInputHelper.ReadRequiredString("Username: ");
        var password = ConsoleInputHelper.ReadPasswordMasked("Password: ");

        try
        {
            var success = await _authService.LoginAsync(username, password);
            if (success)
            {
                ConsoleOutputHelper.PrintSuccess("Login successful!");
                ConsoleInputHelper.WaitForUser();
                return true;
            }
            else
            {
                ConsoleOutputHelper.PrintError("Invalid username or password.");
                ConsoleInputHelper.WaitForUser();
                return false;
            }
        }
        catch (ValidationException ex)
        {
            ConsoleOutputHelper.PrintError(ex.Message);
            ConsoleInputHelper.WaitForUser();
            return false;
        }
        catch (Exception ex)
        {
            ConsoleOutputHelper.PrintError("An unexpected error occurred.");
            ConsoleOutputHelper.PrintDebug(ex.ToString());
            ConsoleInputHelper.WaitForUser();
            return false;
        }
    }
}
