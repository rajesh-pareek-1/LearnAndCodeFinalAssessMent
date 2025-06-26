using NewsSyncConsoleClient.State;
using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class SavedArticlesScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine($"📥 Fetching saved articles for {GlobalAppState.Email}...\n");

            var articles = await FetchSavedArticlesAsync(client);

            if (articles == null || articles.Count == 0)
            {
                Console.WriteLine("ℹ️ No saved articles found.");
                return;
            }

            DisplaySavedArticles(articles);
            await PromptToDeleteArticleAsync(client);
        }

        private static async Task<List<ArticleDto>?> FetchSavedArticlesAsync(HttpClient client)
        {
            var response = await client.GetAsync($"/api/savedArticle?userId={GlobalAppState.UserId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Failed to fetch saved articles. Status: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<ArticleDto>>();
        }

        private static void DisplaySavedArticles(List<ArticleDto> articles)
        {
            Console.WriteLine("📰 Your Saved Articles:\n");

            foreach (var article in articles)
            {
                Console.WriteLine($"🆔 ID: {article.Id}");
                Console.WriteLine($"📰 Title: {article.Headline}");
                Console.WriteLine($"📅 Date: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
                Console.WriteLine($"✍️ Author: {article.AuthorName ?? "N/A"}");
                Console.WriteLine($"📡 Source: {article.Source}");
                Console.WriteLine($"🈚 Language: {article.Language}");
                Console.WriteLine($"🌐 URL: {article.Url}");
                Console.WriteLine($"🖼️ Image: {article.ImageUrl}");
                Console.WriteLine($"📝 Description: {article.Description}");
                Console.WriteLine(new string('-', 80));
            }
        }

        private static async Task PromptToDeleteArticleAsync(HttpClient client)
        {
            Console.Write("\n🗑️ Do you want to delete a saved article? (y/n): ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input != "y") return;

            Console.Write("🔢 Enter Article ID to delete: ");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out var articleId))
            {
                Console.WriteLine("❌ Invalid article ID.");
                return;
            }

            var deleteUrl = $"/api/savedArticle?userId={GlobalAppState.UserId}&articleId={articleId}";
            var response = await client.DeleteAsync(deleteUrl);

            Console.WriteLine(response.IsSuccessStatusCode
                ? "✅ Article deleted successfully."
                : $"❌ Failed to delete article. Status: {response.StatusCode}");
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
