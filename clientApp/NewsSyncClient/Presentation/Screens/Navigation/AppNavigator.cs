using NewsSyncClient.Core.Interfaces;
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

            var input = Console.ReadLine()?.Trim();

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
                    Console.WriteLine("\nExiting the application. Goodbye.");
                    exitRequested = true;
                    break;

                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
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
                Console.WriteLine("\nUnknown role. Access denied.");
                Console.ReadLine();
                break;
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("=== Welcome to NewsSync ===");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Sign Up");
        Console.WriteLine("3. Exit");
        Console.Write("\nEnter your choice: ");
    }
}
