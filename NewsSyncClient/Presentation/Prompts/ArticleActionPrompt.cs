using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Presentation.Helpers;

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
                var reason = ConsoleInputHelper.ReadRequiredString("Enter reason for report: ");
                await ExecuteForArticleAsync(articles, id => _articleService.ReportArticleAsync(id, reason), "Article reported.");
            }),
            ["5"] = () => RenderFilteredAsync(articles, a => a.IsLiked, "Liked Articles"),
            ["6"] = () => RenderFilteredAsync(articles, a => a.IsDisliked, "Disliked Articles"),
            ["7"] = () => Task.CompletedTask
        };

        while (true)
        {
            RenderActionMenu();
            var input = ConsoleInputHelper.ReadOptional("Select an action: ");

            if (input == "7") return;

            if (menuActions.TryGetValue(input ?? string.Empty, out var selectedAction))
                await selectedAction();
            else
                ConsoleOutputHelper.PrintError("Invalid option. Please try again.");
        }
    }

    private async Task ExecuteForArticleAsync(List<ArticleDto> articles, Func<int, Task> action, string confirmationMessage)
    {
        var articleId = ConsoleInputHelper.ReadPositiveInt("Enter Article ID: ");

        if (!articles.Any(a => a.Id == articleId))
            throw new UserInputException($"No article found with ID {articleId}.");

        await action(articleId);
        ConsoleOutputHelper.PrintSuccess(confirmationMessage);
    }

    private Task RenderFilteredAsync(List<ArticleDto> articles, Func<ArticleDto, bool> filter, string title)
    {
        var filtered = articles
            .Where(filter)
            .OrderByDescending(a => a.PublishedDate)
            .ToList();

        ConsoleOutputHelper.PrintHeader(title);
        _articleRenderer.Render(filtered);
        return Task.CompletedTask;
    }

    private void RenderActionMenu()
    {
        ConsoleOutputHelper.PrintHeader("Article Actions");
        ConsoleOutputHelper.PrintInfo("1. Save Article");
        ConsoleOutputHelper.PrintInfo("2. Like Article");
        ConsoleOutputHelper.PrintInfo("3. Dislike Article");
        ConsoleOutputHelper.PrintInfo("4. Report Article");
        ConsoleOutputHelper.PrintInfo("5. Show Liked Articles");
        ConsoleOutputHelper.PrintInfo("6. Show Disliked Articles");
        ConsoleOutputHelper.PrintInfo("7. Back");
    }

    private async Task HandleErrorsAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (UserInputException ex)
        {
            ConsoleOutputHelper.PrintError($"Input Error: {ex.Message}");
        }
        catch (ValidationException ex)
        {
            ConsoleOutputHelper.PrintError($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            ConsoleOutputHelper.PrintError($"Something went wrong: {ex.Message}");
        }
    }
}
