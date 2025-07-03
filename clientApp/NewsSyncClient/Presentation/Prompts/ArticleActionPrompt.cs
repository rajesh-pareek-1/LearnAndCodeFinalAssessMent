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
            ["1"] = () => ExecuteForArticleAsync(articles, _articleService.SaveArticleAsync, "Article saved."),
            ["2"] = () => ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, true), "Article liked."),
            ["3"] = () => ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, false), "Article disliked."),
            ["4"] = async () =>
            {
                Console.Write("Enter reason for report: ");
                var reason = Console.ReadLine();
                await ExecuteForArticleAsync(articles, id => _articleService.ReportArticleAsync(id, reason), "Article reported.");
            },
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
        if (int.TryParse(Console.ReadLine(), out var articleId) && articles.Any(a => a.Id == articleId))
        {
            await action(articleId);
            Console.WriteLine(confirmationMessage);
        }
        else
        {
            Console.WriteLine("Invalid Article ID.");
        }
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

    private void  RenderActionMenu()
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
}
