using NewsSyncConsoleClient.Services;
using NewsSyncConsoleClient.State;
using System.Net.Http.Headers;

namespace NewsSyncClient.Screens
{

    public static class LoginScreen
    {
        public static async Task ShowAsync(AuthService authService)
        {
            Console.Clear();
            Console.WriteLine("=== Login ===\n");

            var email = Prompt("Email: ");
            var password = Prompt("Password: ");

            var success = await authService.LoginAsync(email, password);

            if (!success)
            {
                Console.WriteLine("\nLogin failed. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            var httpClient = CreateAuthorizedClient();

            switch (GlobalAppState.UserRole)
            {
                case "User":
                    await UserDashboardScreen.ShowAsync(httpClient);
                    break;

                case "Admin":
                    await AdminDashboardScreen.ShowAsync(httpClient);
                    break;

                default:
                    Console.WriteLine("\nUnknown role. Access denied.");
                    Console.ReadLine();
                    break;
            }
        }

        private static string Prompt(string label)
        {
            Console.Write(label);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private static HttpClient CreateAuthorizedClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationService.GetApiBaseUrl())
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GlobalAppState.JwtToken);
            return client;
        }
    }
}