using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.UseCases;

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

        var savedArticles = await _savedArticlesFetcher.ExecuteAsync();

        if (savedArticles.Count == 0)
        {
            Console.WriteLine("You haven't saved any articles yet.");
            return;
        }

        _articleRenderer.Render(savedArticles);
        await _deletePrompt.ShowAsync(savedArticles);
    }
}
