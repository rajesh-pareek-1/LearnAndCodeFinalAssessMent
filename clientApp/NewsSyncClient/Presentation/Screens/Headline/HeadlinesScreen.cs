using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

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
            var userSelection = Console.ReadLine();

            if (userSelection == "3") return;

            if (navigationOptions.TryGetValue(userSelection ?? string.Empty, out var selectedAction))
                await selectedAction();
            else
                Console.WriteLine("Invalid selection. Please try again.");

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private async Task ShowWithCustomDateRangeAsync()
    {
        var (startDate, endDate) = ConsoleInputHelper.ReadDateRange();
        if (startDate != null && endDate != null)
            await ShowByDateAsync(startDate.Value, endDate.Value);
    }

    private async Task ShowByDateAsync(DateTime from, DateTime to)
    {
        var selectedCategory = await PromptForCategoryAsync();
        var articles = await _headlineUseCase.ExecuteAsync(from, to, selectedCategory);

        if (!articles.Any())
        {
            Console.WriteLine("No articles found for the selected date range and category.");
            return;
        }

        _articleRenderer.Render(articles);
        await _articleActionPrompt.ShowAsync(articles);
    }

    private async Task<string?> PromptForCategoryAsync()
    {
        var availableCategories = await _headlineUseCase.GetCategoriesAsync();

        if (!availableCategories.Any())
        {
            Console.WriteLine("No categories are currently available.");
            return null;
        }

        Console.WriteLine("\nAvailable Categories:");
        foreach (var category in availableCategories)
            Console.WriteLine($"- {category.Name}");

        return ConsoleInputHelper.ReadOptional("Enter a category (or leave blank to include all): ");
    }

    private void  RenderActionMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Headlines ===");
        Console.WriteLine("1. View Today's Headlines");
        Console.WriteLine("2. View Headlines by Date Range");
        Console.WriteLine("3. Back");
        Console.Write("Select an option: ");
    }
}
