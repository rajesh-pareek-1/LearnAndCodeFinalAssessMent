using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Prompts;

public class SavedArticlePrompt : ISavedArticlePrompt
{
    private readonly ISavedArticleService _savedArticleService;

    public SavedArticlePrompt(ISavedArticleService savedArticleService)
    {
        _savedArticleService = savedArticleService;
    }

    public async Task ShowAsync(List<ArticleDto> savedArticles)
    {
        await HandleUserInputAsync(async () =>
        {
            Console.Write("\nWould you like to delete a saved article? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y") return;

            Console.Write("Enter the Article ID to delete: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var articleId))
                throw new UserInputException("Article ID must be a valid number.");

            if (!savedArticles.Any(article => article.Id == articleId))
                throw new UserInputException($"No saved article found with ID {articleId}.");

            var isDeleted = await _savedArticleService.DeleteSavedArticleAsync(articleId);
            Console.WriteLine(isDeleted ? "Article deleted successfully." : "Failed to delete the article.");
        });
    }

    private async Task HandleUserInputAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (UserInputException ex)
        {
            Console.WriteLine($"Input Error: {ex.Message}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
    }
}
