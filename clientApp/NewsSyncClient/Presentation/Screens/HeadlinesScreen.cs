using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

public class HeadlinesScreen : IHeadlinesScreen
{
    private readonly IFetchHeadlinesUseCase _fetchUseCase;
    private readonly IArticleRenderer _renderer;
    private readonly IArticleActionPrompt _prompt;

    public HeadlinesScreen(
        IFetchHeadlinesUseCase fetchUseCase,
        IArticleRenderer renderer,
        IArticleActionPrompt prompt)
    {
        _fetchUseCase = fetchUseCase;
        _renderer = renderer;
        _prompt = prompt;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Headlines");
            Console.WriteLine("1. Today");
            Console.WriteLine("2. By Date Range");
            Console.WriteLine("3. Back");
            Console.Write("Choice: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ShowByDateAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date);
                    break;
                case "2":
                    var (from, to) = ConsoleInputHelper.ReadDateRange();
                    if (from != null && to != null)
                        await ShowByDateAsync(from.Value, to.Value);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid.");
                    break;
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }
    }

    private async Task ShowByDateAsync(DateTime from, DateTime to)
    {
        var categories = await _fetchUseCase.GetCategoriesAsync();

        if (categories.Any())
        {
            Console.WriteLine("\nAvailable Categories:");
            foreach (var cat in categories)
                Console.WriteLine($"- {cat.Name}");
        }
        else
        {
            Console.WriteLine("No categories available.");
        }

        var category = ConsoleInputHelper.ReadOptional("Enter category (or leave blank): ");
        var articles = await _fetchUseCase.ExecuteAsync(from, to, category);

        if (!articles.Any())
        {
            Console.WriteLine("No articles found.");
            return;
        }

        _renderer.Render(articles);
        await _prompt.ShowAsync(articles);
    }
}
