using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Presentation.Screens;

public class SearchArticlesScreen : ISearchArticlesScreen
{
    private readonly ISearchArticlesUseCase _useCase;
    private readonly IArticleRenderer _renderer;
    private readonly ISearchArticlesPrompt _prompt;

    public SearchArticlesScreen(ISearchArticlesUseCase useCase, IArticleRenderer renderer, ISearchArticlesPrompt prompt)
    {
        _useCase = useCase;
        _renderer = renderer;
        _prompt = prompt;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Article Search ===\n");

        try
        {
            var query = AskSearchTerm();
            if (string.IsNullOrWhiteSpace(query))
                throw new UserInputException("Search term cannot be empty.");

            var articles = await _useCase.ExecuteAsync(query);

            if (articles.Count == 0)
            {
                Console.WriteLine("No articles found.");
                return;
            }

            _renderer.Render(articles);
            await _prompt.PromptToSaveAsync(articles);
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"\n {ex.Message}");
        }

        Console.WriteLine("\nPress Enter to return.");
        Console.ReadLine();
    }

    private string? AskSearchTerm()
    {
        Console.Write("Enter search term: ");
        return Console.ReadLine()?.Trim();
    }
}
