using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Presentation.Screens;

public class SavedArticlesScreen : ISavedArticlesScreen
{
    private readonly IFetchSavedArticlesUseCase _savedArticlesFetcher;
    private readonly IArticleRenderer _articleRenderer;
    private readonly ISavedArticlePrompt _deletePrompt;

    public SavedArticlesScreen(IFetchSavedArticlesUseCase savedArticlesFetcher, IArticleRenderer articleRenderer, ISavedArticlePrompt deletePrompt)
    {
        _savedArticlesFetcher = savedArticlesFetcher;
        _articleRenderer = articleRenderer;
        _deletePrompt = deletePrompt;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Saved Articles ===\n");

        try
        {
            var savedArticles = await _savedArticlesFetcher.ExecuteAsync();

            if (savedArticles == null || savedArticles.Count == 0)
                throw new UserInputException("You haven't saved any articles yet.");

            _articleRenderer.Render(savedArticles);
            await _deletePrompt.ShowAsync(savedArticles);
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"\n {ex.Message}");
        }

        Console.WriteLine("\nPress Enter to return.");
        Console.ReadLine();
    }
}
