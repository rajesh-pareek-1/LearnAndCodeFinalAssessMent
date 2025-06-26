using NewsSyncConsoleClient.State;
using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class AdminDashboardScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome Admin, {GlobalAppState.Email}!");
                Console.WriteLine($"Date: {DateTime.Now:dd-MMM-yyyy}");
                Console.WriteLine($"Time: {DateTime.Now:hh:mmtt}");
                Console.WriteLine("\n=== Admin Dashboard ===");
                Console.WriteLine("1. View the list of external servers and status");
                Console.WriteLine("2. View the external server’s details");
                Console.WriteLine("3. Update/Edit the external server’s API key");
                Console.WriteLine("4. Add new News Category");
                Console.WriteLine("5. Logout");

                Console.Write("\nEnter your choice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ViewServerStatusesAsync(client);
                        break;
                    case "2":
                        await ViewServerDetailsAsync(client);
                        break;
                    case "3":
                        await UpdateServerApiKeyAsync(client);
                        break;
                    case "4":
                        await AddCategoryAsync(client);
                        break;
                    case "5":
                        Logout();
                        return;
                    default:
                        Console.WriteLine("❌ Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        private static async Task ViewServerStatusesAsync(HttpClient client)
        {
            Console.WriteLine("\n🔍 Fetching server statuses...");

            var response = await client.GetAsync("/api/admin/server");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to fetch statuses. Status: {response.StatusCode}");
                return;
            }

            var servers = await response.Content.ReadFromJsonAsync<List<ServerStatusDto>>();

            if (servers == null || servers.Count == 0)
            {
                Console.WriteLine("No servers found.");
                return;
            }

            Console.WriteLine("\n🖥 Server Statuses:");
            foreach (var server in servers)
            {
                Console.WriteLine($"Uptime: {server.Uptime}");
                Console.WriteLine($"Last Accessed: {server.LastAccessed:dd-MMM-yyyy hh:mm tt}");
                Console.WriteLine(new string('-', 40));
            }
        }

        private static async Task ViewServerDetailsAsync(HttpClient client)
        {
            Console.WriteLine("\n🔍 Fetching server details...");

            var response = await client.GetAsync("/api/admin/server/serverDetails");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to fetch server details. Status: {response.StatusCode}");
                return;
            }

            var details = await response.Content.ReadFromJsonAsync<List<ServerDetailsDto>>();

            if (details == null || details.Count == 0)
            {
                Console.WriteLine("No server details available.");
                return;
            }

            Console.WriteLine("\n📡 Server Details:");
            foreach (var server in details)
            {
                Console.WriteLine($"ID: {server.Id}");
                Console.WriteLine($"Server: {server.ServerName}");
                Console.WriteLine($"API Key: {server.ApiKey}");
                Console.WriteLine(new string('-', 40));
            }
        }

        private static async Task UpdateServerApiKeyAsync(HttpClient client)
        {
            Console.Write("\nEnter Server ID to update: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int serverId))
            {
                Console.WriteLine("❌ Invalid server ID.");
                return;
            }

            Console.Write("Enter new API Key: ");
            var newKey = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newKey))
            {
                Console.WriteLine("❌ API Key cannot be empty.");
                return;
            }

            var payload = new { newApiKey = newKey };

            var response = await client.PutAsJsonAsync($"/api/admin/server/{serverId}", payload);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ API key updated successfully.");
            }
            else
            {
                Console.WriteLine($"❌ Failed to update API key. Status: {response.StatusCode}");
            }
        }

        private static async Task AddCategoryAsync(HttpClient client)
        {
            Console.Write("\nEnter category name: ");
            var name = Console.ReadLine();
            Console.Write("Enter category description: ");
            var description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("❌ Category name is required.");
                return;
            }

            var payload = new { name, description };

            var response = await client.PostAsJsonAsync("/api/admin/category", payload);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Category added successfully.");
            }
            else
            {
                Console.WriteLine($"❌ Failed to add category. Status: {response.StatusCode}");
            }
        }

        private static void Logout()
        {
            GlobalAppState.JwtToken = null;
            GlobalAppState.UserId = null;
            GlobalAppState.UserRole = null;
            GlobalAppState.Email = null;
            Console.WriteLine("✅ Logged out successfully.");
        }

        private class ServerStatusDto
        {
            public TimeSpan Uptime { get; set; }
            public DateTime LastAccessed { get; set; }
        }

        private class ServerDetailsDto
        {
            public int Id { get; set; }
            public string ServerName { get; set; } = string.Empty;
            public string ApiKey { get; set; } = string.Empty;
        }
    }
}
