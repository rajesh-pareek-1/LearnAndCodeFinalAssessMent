using NewsSyncConsoleClient.State;
using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class SearchArticlesScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("🔍 Article Search\n");

            Console.Write("Enter search term: ");
            var query = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("❌ Search term cannot be empty.");
                return;
            }

            var articles = await SearchArticlesAsync(client, query);

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("📭 No articles found matching the search term.");
                return;
            }

            DisplayArticles(articles);
            await PromptToSaveArticleAsync(client, articles);
        }

        private static async Task<List<ArticleDto>?> SearchArticlesAsync(HttpClient client, string query)
        {
            var url = $"/api/article/search?query={Uri.EscapeDataString(query)}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to search articles. Status: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<ArticleDto>>();
        }

        private static void DisplayArticles(List<ArticleDto> articles)
        {
            Console.Clear();
            Console.WriteLine("🔍 Search Results:\n");

            foreach (var article in articles)
            {
                Console.WriteLine($"🆔 ID: {article.Id}");
                Console.WriteLine($"📰 Title: {article.Headline}");
                Console.WriteLine($"📅 Published: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
                Console.WriteLine($"✍️ Author: {article.AuthorName ?? "N/A"}");
                Console.WriteLine($"📡 Source: {article.Source}");
                Console.WriteLine($"🈚 Language: {article.Language}");
                Console.WriteLine($"🌐 URL: {article.Url}");
                Console.WriteLine($"🖼️ Image: {article.ImageUrl}");
                Console.WriteLine($"📝 Description: {article.Description}");
                Console.WriteLine(new string('-', 80));
            }
        }

        private static async Task PromptToSaveArticleAsync(HttpClient client, List<ArticleDto> articles)
        {
            Console.Write("\n💾 Do you want to save an article? (y/n): ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input != "y")
                return;

            Console.Write("🔢 Enter Article ID to save: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out var articleId) || !articles.Any(a => a.Id == articleId))
            {
                Console.WriteLine("❌ Invalid article ID.");
                return;
            }

            var payload = new
            {
                articleId,
                userId = GlobalAppState.UserId
            };

            var saveResponse = await client.PostAsJsonAsync("/api/savedArticle", payload);

            Console.WriteLine(saveResponse.IsSuccessStatusCode
                ? "✅ Article saved successfully."
                : $"❌ Failed to save article. Status: {saveResponse.StatusCode}");
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
        }
    }
}
