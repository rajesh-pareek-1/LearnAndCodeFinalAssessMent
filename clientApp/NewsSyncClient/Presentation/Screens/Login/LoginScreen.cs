using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;

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
        Console.WriteLine("=== Login ===\n");

        try
        {
            var email = Prompt("Email: ");
            var password = Prompt("Password: ");

            if (string.IsNullOrWhiteSpace(email))
                throw new UserInputException("Email cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new UserInputException("Password cannot be empty.");

            var success = await _authService.LoginAsync(email, password);

            if (!success)
            {
                Console.WriteLine("\nLogin failed. Press Enter to return.");
                Console.ReadLine();
            }

            return success;
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"\n {ex.Message}");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
            return false;
        }
    }

    private string Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
