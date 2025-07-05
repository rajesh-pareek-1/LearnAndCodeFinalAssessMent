using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Presentation.Helpers;
using NewsSyncClient.Presentation.Screens;

public class AppNavigator
{
    private readonly IAuthService _authService;
    private readonly ISessionContext _session;
    private readonly LoginScreen _loginScreen;
    private readonly SignupScreen _signupScreen;
    private readonly UserDashboardScreen _userDashboard;
    private readonly AdminDashboardScreen _adminDashboard;

    public AppNavigator(IAuthService authService, ISessionContext session, LoginScreen loginScreen, SignupScreen signupScreen, UserDashboardScreen userDashboard, AdminDashboardScreen adminDashboard)
    {
        _authService = authService;
        _session = session;
        _loginScreen = loginScreen;
        _signupScreen = signupScreen;
        _userDashboard = userDashboard;
        _adminDashboard = adminDashboard;
    }

    public async Task StartAsync()
    {
        var exitRequested = false;

        while (!exitRequested)
        {
            Console.Clear();
            ShowMainMenu();

            var input = ConsoleInputHelper.ReadOptional("\nEnter your choice: ");

            switch (input)
            {
                case "1":
                    var loggedIn = await _loginScreen.ShowAsync();
                    if (loggedIn)
                        await NavigateToDashboardAsync();
                    break;

                case "2":
                    await _signupScreen.ShowAsync();
                    break;

                case "3":
                    ConsoleOutputHelper.PrintInfo("\nExiting the application. Goodbye.");
                    exitRequested = true;
                    break;

                default:
                    ConsoleOutputHelper.PrintError("\nInvalid option. Please try again.");
                    await Task.Delay(1500);
                    break;
            }
        }
    }

    private async Task NavigateToDashboardAsync()
    {
        switch (_session.Role)
        {
            case "User":
                await _userDashboard.ShowAsync();
                break;
            case "Admin":
                await _adminDashboard.ShowAsync();
                break;
            default:
                ConsoleOutputHelper.PrintError("\nUnknown role. Access denied.");
                ConsoleInputHelper.ReadOptional("Press Enter to return...");
                break;
        }
    }

    private void ShowMainMenu()
    {
        ConsoleOutputHelper.PrintHeader("Welcome to NewsSync");
        ConsoleOutputHelper.PrintInfo("1. Login");
        ConsoleOutputHelper.PrintInfo("2. Sign Up");
        ConsoleOutputHelper.PrintInfo("3. Exit");
    }
}
