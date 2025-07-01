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

        var email = Prompt("Email: ");
        var password = Prompt("Password: ");

        var success = await _authService.LoginAsync(email, password);

        if (!success)
        {
            Console.WriteLine("\nLogin failed. Press Enter to return.");
            Console.ReadLine();
        }

        return success;
    }

    private string Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
