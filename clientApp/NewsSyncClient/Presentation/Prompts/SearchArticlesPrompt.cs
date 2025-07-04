using NewsSyncClient.Core.Exceptions;
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
        var decision = Console.ReadLine()?.Trim().ToLower();

        if (decision != "y") return;

        Console.Write("Enter Article ID to save: ");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new UserInputException("Article ID input cannot be empty.");

        if (!int.TryParse(input, out var id))
            throw new UserInputException("Invalid number format. Please enter a numeric Article ID.");

        if (!articles.Any(a => a.Id == id))
            throw new UserInputException($"No article found with ID: {id}");

        await _savedService.SaveArticleAsync(id);
        Console.WriteLine("Article saved successfully.");
    }
}
