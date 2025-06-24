using NewsSyncConsoleClient.Services;

namespace NewsSyncClient.Screens
{
    public static class AppNavigator
    {
        public static async Task RenderHomeScreenAsync(AuthService authService, HttpClient httpClient)
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                Console.Clear();
                ShowMenu();

                var choice = Console.ReadLine()?.Trim();

                
                switch (choice)
                {
                    case "1":
                        await LoginScreen.ShowAsync(authService);
                        break;

                    case "2":
                        await SignupScreen.ShowAsync(httpClient);
                        break;

                    case "3":
                        Console.WriteLine("\n👋 Exiting the application. Goodbye!");
                        exitRequested = true;
                        break;

                    default:
                        Console.WriteLine("\n❌ Invalid option. Please try again.");
                        await Task.Delay(1500);
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("=== 📰 Welcome to News Aggregator ===\n");
            Console.WriteLine("1. 🔐 Login");
            Console.WriteLine("2. 🆕 Sign up");
            Console.WriteLine("3. 🚪 Exit");
            Console.Write("\nEnter your choice: ");
        }
    }
}
