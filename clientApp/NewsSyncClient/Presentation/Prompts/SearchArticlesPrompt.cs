using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Prompts;

public class SearchArticlesPrompt : ISearchArticlesPrompt
{
    private readonly ISavedArticleService _savedService;

    public SearchArticlesPrompt(ISavedArticleService savedService)
    {
        _savedService = savedService;
    }

    public async Task PromptToSaveAsync(List<ArticleDto> articles)
    {
        Console.Write("\nDo you want to save an article? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        Console.Write("Enter Article ID to save: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var id) || !articles.Any(a => a.Id == id))
        {
            Console.WriteLine("Invalid article ID.");
            return;
        }

        await _savedService.SaveArticleAsync(id);
        Console.WriteLine("Article saved.");
    }
}
