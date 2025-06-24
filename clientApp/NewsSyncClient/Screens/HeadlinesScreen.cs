using NewsSyncConsoleClient.State;
using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class HeadlinesScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"👋 Welcome {GlobalAppState.Email}");
                Console.WriteLine($"📅 {DateTime.Now:dd-MMM-yyyy} ⏰ {DateTime.Now:hh:mmtt}");
                Console.WriteLine("\n📑 View Headlines:");
                Console.WriteLine("1. Today");
                Console.WriteLine("2. By Date Range");
                Console.WriteLine("3. Back");
                Console.Write("\nEnter your choice: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await FetchTodayHeadlinesAsync(client);
                        break;
                    case "2":
                        await FetchDateRangeHeadlinesAsync(client);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("❌ Invalid input. Try again.");
                        break;
                }

                Console.WriteLine("\n🔁 Press Enter to return...");
                Console.ReadLine();
            }
        }

        private static async Task FetchTodayHeadlinesAsync(HttpClient client)
        {
            var category = await AskCategoryAsync(client);
            var today = DateTime.UtcNow.Date;

            var url = $"/api/article?fromDate={today:yyyy-MM-dd}&toDate={today.AddDays(1):yyyy-MM-dd}";
            if (!string.IsNullOrWhiteSpace(category))
                url += $"&category={Uri.EscapeDataString(category)}";

            await FetchAndDisplayArticlesAsync(client, url, "today");
        }

        private static async Task FetchDateRangeHeadlinesAsync(HttpClient client)
        {
            Console.Write("📅 From Date (yyyy-mm-dd): ");
            var fromInput = Console.ReadLine();
            Console.Write("📅 To Date (yyyy-mm-dd): ");
            var toInput = Console.ReadLine();

            if (!DateTime.TryParse(fromInput, out var fromDate) || !DateTime.TryParse(toInput, out var toDate))
            {
                Console.WriteLine("⚠️ Invalid date format.");
                return;
            }

            var category = await AskCategoryAsync(client);
            var url = $"/api/article?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate.AddDays(1):yyyy-MM-dd}";
            if (!string.IsNullOrWhiteSpace(category))
                url += $"&category={Uri.EscapeDataString(category)}";

            await FetchAndDisplayArticlesAsync(client, url, "selected range");
        }

        private static async Task FetchAndDisplayArticlesAsync(HttpClient client, string url, string context)
        {
            Console.WriteLine($"\n🔍 Fetching articles for {context}...");

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to fetch articles: {response.StatusCode}");
                return;
            }

            var articles = await response.Content.ReadFromJsonAsync<List<ArticleDto>>();

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("⚠️ No articles found.");
                return;
            }

            DisplayArticles(articles);
            await ShowArticleActionsAsync(client, articles);
        }

        private static void DisplayArticles(List<ArticleDto> articles)
        {
            Console.Clear();
            Console.WriteLine("📰 Headlines:\n");

            foreach (var article in articles)
            {
                Console.WriteLine($"🆔 ID: {article.Id}");
                Console.WriteLine($"🧾 Title: {article.Headline}");
                Console.WriteLine($"📅 Date: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
                Console.WriteLine($"✍️ Author: {article.AuthorName ?? "N/A"}");
                Console.WriteLine($"📡 Source: {article.Source}");
                Console.WriteLine($"🌐 URL: {article.Url}");
                Console.WriteLine($"🖼️ Image: {article.ImageUrl}");
                Console.WriteLine($"🈚 Language: {article.Language}");
                Console.WriteLine($"📝 Description: {article.Description}");
                Console.WriteLine(new string('-', 80));
            }
        }

        private static async Task ShowArticleActionsAsync(HttpClient client, List<ArticleDto> articles)
        {
            while (true)
            {
                Console.WriteLine("\nActions:");
                Console.WriteLine("1. Save an Article");
                Console.WriteLine("2. Like an Article");
                Console.WriteLine("3. Dislike an Article");
                Console.WriteLine("4. Report an Article");
                Console.WriteLine("5. Sort by Likes (Client-side)");
                Console.WriteLine("6. Back");

                Console.Write("\nEnter your choice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await HandleSaveArticleAsync(client, articles);
                        break;
                    case "2":
                        await HandleReactionAsync(client, articles, isLiked: true);
                        break;
                    case "3":
                        await HandleReactionAsync(client, articles, isLiked: false);
                        break;
                    case "4":
                        await HandleReportArticleAsync(client, articles);
                        break;
                    case "5":
                        var sorted = articles
                            .OrderByDescending(a => a.LikeCount) // needs to be part of ArticleDto
                            .ToList();
                        DisplayArticles(sorted);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("❌ Invalid choice.");
                        break;
                }
            }
        }


        private static async Task<string?> AskCategoryAsync(HttpClient client)
        {
            Console.WriteLine("\n📂 Fetching categories...");

            var response = await client.GetAsync("/api/categories/article");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Failed to fetch categories.");
                return null;
            }

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();

            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("⚠️ No categories found.");
                return null;
            }

            Console.WriteLine("\nAvailable Categories:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"- {cat.Name}");
            }

            Console.Write("\nEnter category name to filter (or leave blank to skip): ");
            var input = Console.ReadLine();


            return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        private static async Task HandleSaveArticleAsync(HttpClient client, List<ArticleDto> articles)
        {
            Console.Write("Enter Article ID to save: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out var articleId) || !articles.Any(a => a.Id == articleId))
            {
                Console.WriteLine("❌ Invalid Article ID.");
                return;
            }

            var payload = new { articleId, userId = GlobalAppState.UserId };
            var response = await client.PostAsJsonAsync("/api/savedArticle", payload);

            Console.WriteLine(response.IsSuccessStatusCode ? "✅ Saved." : $"❌ Error: {response.StatusCode}");
        }


        private static async Task HandleReactionAsync(HttpClient client, List<ArticleDto> articles, bool isLiked)
        {
            Console.Write($"Enter Article ID to {(isLiked ? "like" : "dislike")}: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out var articleId) || !articles.Any(a => a.Id == articleId))
            {
                Console.WriteLine("❌ Invalid Article ID.");
                return;
            }

            var payload = new { ArticleId = articleId, UserId = GlobalAppState.UserId, IsLiked = isLiked };
            var response = await client.PostAsJsonAsync("/api/article/reaction", payload);

            Console.WriteLine(response.IsSuccessStatusCode ? "✅ Reaction updated." : $"❌ Failed: {response.StatusCode}");
        }



        private static async Task HandleReportArticleAsync(HttpClient client, List<ArticleDto> articles)
        {
            Console.Write("Enter Article ID to report: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out var articleId) || !articles.Any(a => a.Id == articleId))
            {
                Console.WriteLine("❌ Invalid Article ID.");
                return;
            }

            Console.Write("Reason for reporting (optional): ");
            var reason = Console.ReadLine();

            var payload = new { ArticleId = articleId, UserId = GlobalAppState.UserId, Reason = reason };
            var response = await client.PostAsJsonAsync("/api/article/report", payload);

            Console.WriteLine(response.IsSuccessStatusCode ? "✅ Report submitted." : $"❌ Error: {response.StatusCode}");
        }



        private class ArticleDto
        {
            public int Id { get; set; }
            public string Headline { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Source { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
            public int CategoryId { get; set; }
            public string? AuthorName { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public string Language { get; set; } = string.Empty;
            public DateTime PublishedDate { get; set; }

            public int LikeCount { get; set; } // 💡 Add this
            public int DislikeCount { get; set; } // 💡 Add this
        }


        private class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }
}



