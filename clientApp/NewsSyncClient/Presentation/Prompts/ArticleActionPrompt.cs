using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Prompts;

public class ArticleActionPrompt : IArticleActionPrompt
{
    private readonly IArticleInteractionService _articleService;
    private readonly IArticleRenderer _articleRenderer;

    public ArticleActionPrompt(IArticleInteractionService articleService, IArticleRenderer articleRenderer)
    {
        _articleService = articleService;
        _articleRenderer = articleRenderer;
    }

    public async Task ShowAsync(List<ArticleDto> articles)
    {
        var menuActions = new Dictionary<string, Func<Task>>
        {
            ["1"] = () => HandleErrorsAsync(() => ExecuteForArticleAsync(articles, _articleService.SaveArticleAsync, "Article saved.")),
            ["2"] = () => HandleErrorsAsync(() => ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, true), "Article liked.")),
            ["3"] = () => HandleErrorsAsync(() => ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, false), "Article disliked.")),
            ["4"] = () => HandleErrorsAsync(async () =>
            {
                Console.Write("Enter reason for report: ");
                var reason = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(reason))
                    throw new UserInputException("Report reason cannot be empty.");

                await ExecuteForArticleAsync(articles, id => _articleService.ReportArticleAsync(id, reason), "Article reported.");
            }),
            ["5"] = () => RenderFilteredAsync(articles, a => a.IsLiked, "Liked Articles"),
            ["6"] = () => RenderFilteredAsync(articles, a => a.IsDisliked, "Disliked Articles"),
            ["7"] = () => Task.CompletedTask
        };

        while (true)
        {
            RenderActionMenu();
            var input = Console.ReadLine();

            if (input == "7") return;

            if (menuActions.TryGetValue(input ?? string.Empty, out var selectedAction))
                await selectedAction();
            else
                Console.WriteLine("Invalid option. Please try again.");
        }
    }

    private async Task ExecuteForArticleAsync(List<ArticleDto> articles, Func<int, Task> action, string confirmationMessage)
    {
        Console.Write("Enter Article ID: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var articleId))
            throw new UserInputException("Article ID must be a valid number.");

        if (!articles.Any(a => a.Id == articleId))
            throw new UserInputException($"No article found with ID {articleId}.");

        await action(articleId);
        Console.WriteLine(confirmationMessage);
    }

    private Task RenderFilteredAsync(List<ArticleDto> articles, Func<ArticleDto, bool> filter, string title)
    {
        var filtered = articles
            .Where(filter)
            .OrderByDescending(a => a.PublishedDate)
            .ToList();

        Console.WriteLine($"\n{title}\n");
        _articleRenderer.Render(filtered);
        return Task.CompletedTask;
    }

    private void RenderActionMenu()
    {
        Console.WriteLine("\n=== Article Actions ===");
        Console.WriteLine("1. Save Article");
        Console.WriteLine("2. Like Article");
        Console.WriteLine("3. Dislike Article");
        Console.WriteLine("4. Report Article");
        Console.WriteLine("5. Show Liked Articles");
        Console.WriteLine("6. Show Disliked Articles");
        Console.WriteLine("7. Back");
        Console.Write("Select an action: ");
    }

    private async Task HandleErrorsAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"Input Error: {ex.Message}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
    }
}
