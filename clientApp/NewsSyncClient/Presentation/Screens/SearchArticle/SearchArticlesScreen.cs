using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Presentation.Helpers;

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
        ConsoleOutputHelper.PrintHeader("Article Search");

        try
        {
            var query = ConsoleInputHelper.ReadRequiredString("Enter search term: ");
            var articles = await _useCase.ExecuteAsync(query);

            if (articles.Count == 0)
            {
                ConsoleOutputHelper.PrintWarning("No articles found.");
                return;
            }

            _renderer.Render(articles);
            await _prompt.PromptToSaveAsync(articles);
        }
        catch (UserInputException ex)
        {
            ConsoleOutputHelper.PrintWarning(ex.Message);
        }

        ConsoleInputHelper.ReadOptional("\nPress Enter to return...");
    }
}
