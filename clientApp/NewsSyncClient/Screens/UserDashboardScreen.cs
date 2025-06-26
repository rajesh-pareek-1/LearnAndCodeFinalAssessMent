using NewsSyncConsoleClient.Services;
using NewsSyncConsoleClient.State;

namespace NewsSyncClient.Screens
{
    public static class UserDashboardScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            while (true)
            {
                Console.Clear();
                RenderHeader();

                Console.WriteLine("📋 Please choose an option:");
                Console.WriteLine("1️⃣  Headlines");
                Console.WriteLine("2️⃣  Saved Articles");
                Console.WriteLine("3️⃣  Search Articles");
                Console.WriteLine("4️⃣  Notifications");
                Console.WriteLine("5️⃣  Logout");

                Console.Write("\nEnter your choice: ");
                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        await HeadlinesScreen.ShowAsync(client);
                        break;
                    case "2":
                        await SavedArticlesScreen.ShowAsync(client);
                        break;
                    case "3":
                        await SearchArticlesScreen.ShowAsync(client);
                        break;
                    case "4":
                        await NotificationsScreen.ShowAsync(client);
                        break;
                    case "5":
                        PerformLogout();
                        return;
                    default:
                        Console.WriteLine("❌ Invalid input. Try again.");
                        break;
                }

                Console.WriteLine("\nPress Enter to return to the dashboard...");
                Console.ReadLine();
            }
        }

        private static void RenderHeader()
        {
            Console.WriteLine($"👋 Welcome, {GlobalAppState.Email}!");
            Console.WriteLine($"📅 Date: {DateTime.Now:dd-MMM-yyyy}");
            Console.WriteLine($"⏰ Time: {DateTime.Now:hh:mm tt}\n");
        }

        private static void PerformLogout()
        {
            Console.WriteLine("\n🚪 Logging out...");

            GlobalAppState.JwtToken = null;
            GlobalAppState.UserId = null;
            GlobalAppState.UserRole = null;
            GlobalAppState.Email = null;
        }
    }
}
