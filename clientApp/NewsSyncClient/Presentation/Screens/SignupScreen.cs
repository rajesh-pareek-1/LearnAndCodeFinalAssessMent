using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Presentation.Screens;

public class SignupScreen
{
    private readonly ISignupService _signupService;

    public SignupScreen(ISignupService signupService)
    {
        _signupService = signupService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Sign Up ===\n");

        var email = Prompt("Email: ");
        var password = Prompt("Password: ");

        var dto = new SignupRequestDto
        {
            Username = email,
            Password = password
        };

        var (success, message) = await _signupService.RegisterAsync(dto);
        Console.WriteLine($"\n{message}");

        Console.WriteLine("\nPress Enter to return.");
        Console.ReadLine();
    }

    private static string Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
