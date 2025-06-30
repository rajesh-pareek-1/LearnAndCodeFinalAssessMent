using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Screens;

public class SearchArticlesScreen : ISearchArticlesScreen
{
    private readonly ISearchArticleService _searchService;
    private readonly ISavedArticleService _savedService;

    public SearchArticlesScreen(ISearchArticleService searchService, ISavedArticleService savedService)
    {
        _searchService = searchService;
        _savedService = savedService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("🔍 Article Search\n");

        Console.Write("Enter search term: ");
        var query = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Search term cannot be empty.");
            return;
        }

        var articles = await _searchService.SearchAsync(query);

        if (articles == null || articles.Count == 0)
        {
            Console.WriteLine("No articles found matching the search term.");
            return;
        }

        DisplayArticles(articles);
        await PromptToSaveArticleAsync(articles);
    }

    private void DisplayArticles(List<ArticleDto> articles)
    {
        Console.Clear();
        Console.WriteLine("Search Results:\n");

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

    private async Task PromptToSaveArticleAsync(List<ArticleDto> articles)
    {
        Console.Write("\nDo you want to save an article? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        Console.Write("Enter Article ID to save: ");
        var idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out var articleId) || !articles.Any(a => a.Id == articleId))
        {
            Console.WriteLine("Invalid article ID.");
            return;
        }

        await _savedService.SaveArticleAsync(articleId);
    }
}
