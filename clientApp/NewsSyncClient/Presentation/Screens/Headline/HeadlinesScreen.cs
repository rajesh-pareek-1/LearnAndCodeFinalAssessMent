using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Screens;

public class HeadlinesScreen : IHeadlinesScreen
{
    private readonly IFetchHeadlinesUseCase _headlineUseCase;
    private readonly IArticleRenderer _articleRenderer;
    private readonly IArticleActionPrompt _articleActionPrompt;

    public HeadlinesScreen(IFetchHeadlinesUseCase headlineUseCase, IArticleRenderer articleRenderer, IArticleActionPrompt articleActionPrompt)
    {
        _headlineUseCase = headlineUseCase;
        _articleRenderer = articleRenderer;
        _articleActionPrompt = articleActionPrompt;
    }

    public async Task ShowAsync()
    {
        var navigationOptions = new Dictionary<string, Func<Task>>
        {
            ["1"] = () => ShowByDateAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date),
            ["2"] = ShowWithCustomDateRangeAsync,
            ["3"] = () => Task.CompletedTask
        };

        while (true)
        {
            RenderActionMenu();
            var userSelection = ConsoleInputHelper.ReadOptional("");

            if (userSelection == "3") return;

            if (navigationOptions.TryGetValue(userSelection ?? string.Empty, out var selectedAction))
                await selectedAction();
            else
                ConsoleOutputHelper.PrintError("Invalid selection. Please try again.");

            ConsoleInputHelper.WaitForUser();
        }
    }

    private async Task ShowWithCustomDateRangeAsync()
    {
        var (startDate, endDate) = ConsoleInputHelper.ReadDateRange();
        await ShowByDateAsync(startDate, endDate);
    }

    private async Task ShowByDateAsync(DateTime from, DateTime to)
    {
        var articles = await _headlineUseCase.ExecuteAsync(from, to);

        if (!articles.Any())
        {
            ConsoleOutputHelper.PrintWarning("No articles found for the selected date range and category.");
            return;
        }

        _articleRenderer.Render(articles);
        await _articleActionPrompt.ShowAsync(articles);
    }

    private void RenderActionMenu()
    {
        Console.Clear();
        ConsoleOutputHelper.PrintHeading("Headlines");
        ConsoleOutputHelper.PrintInfo("1. View Today's Headlines");
        ConsoleOutputHelper.PrintInfo("2. View Headlines by Date Range");
        ConsoleOutputHelper.PrintInfo("3. Back");
        ConsoleOutputHelper.PrintInfo("Select an option: ", inline: true);
    }
}
