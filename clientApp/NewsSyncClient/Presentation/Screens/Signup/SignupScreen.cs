using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Auth;
using NewsSyncClient.Presentation.Helpers;

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
        ConsoleOutputHelper.PrintHeader("Sign Up");

        try
        {
            var email = ConsoleInputHelper.ReadRequiredString("Email: ");
            var password = ConsoleInputHelper.ReadRequiredString("Password: ");

            var dto = new SignupRequestDto { Username = email, Password = password };
            var (success, message) = await _signupService.RegisterAsync(dto);

            ConsoleOutputHelper.PrintInfo($"\n{message}");
        }
        catch (UserInputException ex)
        {
            ConsoleOutputHelper.PrintError(ex.Message);
        }

        ConsoleInputHelper.ReadOptional("\nPress Enter to return...");
    }
}
