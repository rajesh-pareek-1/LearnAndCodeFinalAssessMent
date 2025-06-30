using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Screens;

public class SavedArticlesScreen : ISavedArticlesScreen
{
    private readonly ISessionContext _session;
    private readonly ISavedArticleService _savedService;

    public SavedArticlesScreen(ISessionContext session, ISavedArticleService savedService)
    {
        _session = session;
        _savedService = savedService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine($"Fetching saved articles for {_session.Email}...\n");

        var articles = await _savedService.GetSavedArticlesAsync();

        if (articles == null || articles.Count == 0)
        {
            Console.WriteLine("No saved articles found.");
            return;
        }

        DisplayArticles(articles);
        await AskToDeleteAsync(articles);
    }

    private void DisplayArticles(List<ArticleDto> articles)
    {
        Console.WriteLine("Your Saved Articles:\n");

        foreach (var article in articles)
        {
            Console.WriteLine($"ID: {article.Id}");
            Console.WriteLine($"Title: {article.Headline}");
            Console.WriteLine($"Date: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine($"Author: {article.AuthorName ?? "N/A"}");
            Console.WriteLine($"Source: {article.Source}");
            Console.WriteLine($"Language: {article.Language}");
            Console.WriteLine($"URL: {article.Url}");
            Console.WriteLine($"Image: {article.ImageUrl}");
            Console.WriteLine($"Description: {article.Description}");
            Console.WriteLine(new string('-', 80));
        }
    }

    private async Task AskToDeleteAsync(List<ArticleDto> articles)
    {
        Console.Write("\nDo you want to delete a saved article? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        Console.Write("Enter Article ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id) || !articles.Any(a => a.Id == id))
        {
            Console.WriteLine("Invalid Article ID.");
            return;
        }

        var success = await _savedService.DeleteSavedArticleAsync(id);
        Console.WriteLine(success ? "Deleted." : "Failed to delete article.");
    }
}
