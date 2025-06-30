using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Screens;

public class HeadlinesScreen : IHeadlinesScreen
{
    private readonly ISessionContext _session;
    private readonly IArticleInteractionService _articleService;

    public HeadlinesScreen(ISessionContext session, IArticleInteractionService articleService)
    {
        _session = session;
        _articleService = articleService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            ShowHeader();

            Console.WriteLine("View Headlines:");
            Console.WriteLine("1. Today");
            Console.WriteLine("2. By Date Range");
            Console.WriteLine("3. Back");
            Console.Write("\nEnter your choice: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ShowHeadlinesAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
                    break;
                case "2":
                    await HandleDateRangeAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }
    }

    private void ShowHeader()
    {
        Console.WriteLine($"Welcome, {_session.Email}");
        Console.WriteLine($"Date: {DateTime.Now:dd-MMM-yyyy} | Time: {DateTime.Now:hh:mm tt}\n");
    }

    private async Task HandleDateRangeAsync()
    {
        Console.Write("From Date (yyyy-mm-dd): ");
        var fromInput = Console.ReadLine();
        Console.Write("To Date (yyyy-mm-dd): ");
        var toInput = Console.ReadLine();

        if (!DateTime.TryParse(fromInput, out var from) || !DateTime.TryParse(toInput, out var to))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        await ShowHeadlinesAsync(from, to);
    }

    private async Task ShowHeadlinesAsync(DateTime from, DateTime to)
    {
        Console.WriteLine($"from: {from}, To: {to}");

        var category = await AskCategoryAsync();
        Console.WriteLine($"category passing: {category}");
        var articles = await _articleService.FetchHeadlinesAsync(from, to, category);

        if (articles == null || articles.Count == 0)
        {
            Console.WriteLine("\nNo articles found.");
            return;
        }

        DisplayArticles(articles);
        await ShowArticleActionsAsync(articles);
    }

    private async Task<string> AskCategoryAsync()
    {
        var categories = await _articleService.FetchCategoriesAsync();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found.");
            return null;
        }

        Console.WriteLine("\nAvailable Categories:");
        foreach (var cat in categories)
            Console.WriteLine($"- {cat.Name}");

        Console.Write("\nEnter category (or leave blank): ");
        return Console.ReadLine()?.Trim();
    }

    private void DisplayArticles(List<ArticleDto> articles)
    {
        Console.Clear();
        Console.WriteLine("Headlines:\n");

        foreach (var article in articles)
        {
            Console.WriteLine($"ID: {article.Id}");
            Console.WriteLine($"Title: {article.Headline}");
            Console.WriteLine($"Date: {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine($"Author: {article.AuthorName ?? "N/A"}");
            Console.WriteLine($"Source: {article.Source}");
            Console.WriteLine($"URL: {article.Url}");
            Console.WriteLine($"Image: {article.ImageUrl}");
            Console.WriteLine($"Language: {article.Language}");
            Console.WriteLine($"Description: {article.Description}");
            Console.WriteLine(new string('-', 60));
        }
    }

    private async Task ShowArticleActionsAsync(List<ArticleDto> articles)
    {
        while (true)
        {
            Console.WriteLine("\nActions:");
            Console.WriteLine("1. Save");
            Console.WriteLine("2. Like");
            Console.WriteLine("3. Dislike");
            Console.WriteLine("4. Report");
            Console.WriteLine("5. Show Liked Articles");
            Console.WriteLine("6. Show Disliked Articles");
            Console.WriteLine("7. Back");

            Console.Write("\nChoose: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await SaveArticleAsync(articles);
                    break;
                case "2":
                    await ReactToArticleAsync(articles, true);
                    break;
                case "3":
                    await ReactToArticleAsync(articles, false);
                    break;
                case "4":
                    await ReportArticleAsync(articles);
                    break;
                case "5":
                    var likedArticles = articles.Where(a => a.IsLiked).ToList();
                    Console.WriteLine("\nArticles You Liked:\n");
                    DisplayArticles(likedArticles);
                    break;
                case "6":
                    var dislikedArticles = articles.Where(a => a.IsDisliked).ToList();
                    Console.WriteLine("\nArticles You Disliked:\n");
                    DisplayArticles(dislikedArticles);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }


    private async Task SaveArticleAsync(List<ArticleDto> articles)
    {
        Console.Write("Article ID to save: ");
        if (int.TryParse(Console.ReadLine(), out var id) && articles.Any(a => a.Id == id))
        {
            await _articleService.SaveArticleAsync(id);
            Console.WriteLine("Saved.");
        }
        else
        {
            Console.WriteLine("Invalid Article ID.");
        }
    }

    private async Task ReactToArticleAsync(List<ArticleDto> articles, bool isLiked)
    {
        Console.Write($"Article ID to {(isLiked ? "like" : "dislike")}: ");
        if (int.TryParse(Console.ReadLine(), out var id) && articles.Any(a => a.Id == id))
        {
            await _articleService.ReactToArticleAsync(id, isLiked);
            Console.WriteLine("Reaction recorded.");
        }
        else
        {
            Console.WriteLine("Invalid Article ID.");
        }
    }

    private async Task ReportArticleAsync(List<ArticleDto> articles)
    {
        Console.Write("Article ID to report: ");
        if (int.TryParse(Console.ReadLine(), out var id) && articles.Any(a => a.Id == id))
        {
            Console.Write("Reason (optional): ");
            var reason = Console.ReadLine();
            await _articleService.ReportArticleAsync(id, reason);
            Console.WriteLine("Reported.");
        }
        else
        {
            Console.WriteLine("Invalid Article ID.");
        }
    }
}
