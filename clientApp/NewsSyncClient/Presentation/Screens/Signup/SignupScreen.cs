using NewsSyncClient.Core.Exceptions;
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

        try
        {
            var email = Prompt("Email: ");
            var password = Prompt("Password: ");

            if (string.IsNullOrWhiteSpace(email))
                throw new UserInputException("Email cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new UserInputException("Password cannot be empty.");

            var dto = new SignupRequestDto
            {
                Username = email,
                Password = password
            };

            var (success, message) = await _signupService.RegisterAsync(dto);
            Console.WriteLine($"\n{message}");
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"\n {ex.Message}");
        }

        Console.WriteLine("\nPress Enter to return.");
        Console.ReadLine();
    }

    private static string Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
