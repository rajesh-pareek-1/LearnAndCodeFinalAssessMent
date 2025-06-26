using NewsSyncConsoleClient.State;
using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class NotificationsScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            while (true)
            {
                Console.Clear();
                DisplayHeader();

                Console.WriteLine("1. 🔔 View Notifications");
                Console.WriteLine("2. ⚙️ Configure Notifications");
                Console.WriteLine("3. 🔙 Back");

                Console.Write("\nEnter your choice: ");
                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        await ViewNotificationsAsync(client);
                        break;
                    case "2":
                        await ConfigureNotificationsAsync(client);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("❌ Invalid option. Try again.");
                        break;
                }

                Console.WriteLine("\n🔁 Press Enter to return to the menu...");
                Console.ReadLine();
            }
        }

        private static void DisplayHeader()
        {
            Console.WriteLine($"👤 Welcome {GlobalAppState.Email}");
            Console.WriteLine($"📅 {DateTime.Now:dd-MMM-yyyy} ⏰ {DateTime.Now:hh:mmtt}");
            Console.WriteLine("\n=== 🔔 N O T I F I C A T I O N S ===\n");
        }

        private static async Task ViewNotificationsAsync(HttpClient client)
        {
            var url = $"/api/notification?userId={GlobalAppState.UserId}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to fetch notifications. Status: {response.StatusCode}");
                return;
            }

            var notifications = await response.Content.ReadFromJsonAsync<List<NotificationDto>>();

            if (notifications == null || notifications.Count == 0)
            {
                Console.WriteLine("📭 No notifications found.");
                return;
            }

            Console.WriteLine("\n📬 Your Notifications:\n");

            foreach (var notification in notifications)
            {
                DisplayNotification(notification);
            }
        }

        private static void DisplayNotification(NotificationDto notification)
        {
            var article = notification.Article;

            Console.WriteLine($"🔔 Notification ID: {notification.Id}");
            Console.WriteLine($"📰 Title: {article.Headline}");
            Console.WriteLine($"📅 Published: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine($"✍️ Author: {article.AuthorName ?? "N/A"}");
            Console.WriteLine($"📡 Source: {article.Source}");
            Console.WriteLine($"🈚 Language: {article.Language}");
            Console.WriteLine($"🌐 URL: {article.Url}");
            Console.WriteLine($"🖼️ Image: {article.ImageUrl}");
            Console.WriteLine($"📝 Description: {article.Description}");
            Console.WriteLine($"📨 Sent At: {notification.SentAt:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine(new string('-', 80));
        }

        private static async Task ConfigureNotificationsAsync(HttpClient client)
        {
            // 🛠 Step 1: Fetch categories from correct endpoint
            var categoriesResponse = await client.GetAsync("/api/categories/article");

            if (!categoriesResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to load categories. Status: {categoriesResponse.StatusCode}");
                return;
            }

            var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("📭 No categories available to configure.");
                return;
            }

            // 📋 Step 2: Show available categories
            Console.WriteLine("\n📂 Available Categories:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"- {cat.Name} ({cat.Description})");
            }

            // 🧾 Step 3: Ask user to select one
            Console.Write("\n📌 Enter category name to toggle: ");
            var categoryName = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("⚠️ Category name is required.");
                return;
            }

            Console.Write("✅ Enable notifications for this category? (y/n): ");
            var enableInput = Console.ReadLine()?.Trim().ToLower();

            if (enableInput != "y" && enableInput != "n")
            {
                Console.WriteLine("❌ Invalid input. Use 'y' or 'n'.");
                return;
            }

            bool enable = enableInput == "y";

            // 📤 Step 4: Send update to backend
            var payload = new
            {
                userId = GlobalAppState.UserId,
                categoryName,
                enabled = enable
            };

            var response = await client.PutAsJsonAsync("/api/notification/configure", payload);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Notification configuration updated.");
            }
            else
            {
                Console.WriteLine($"❌ Failed to update configuration. Status: {response.StatusCode}");
            }
        }



        // DTOs scoped internally for encapsulation
        private class NotificationDto
        {
            public int Id { get; set; }
            public DateTime SentAt { get; set; }
            public ArticleDto Article { get; set; } = new();
        }

        private class ArticleDto
        {
            public int Id { get; set; }
            public string Headline { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Source { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
            public string? AuthorName { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public string Language { get; set; } = string.Empty;
            public DateTime PublishedDate { get; set; }
        }

        private class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }
}
