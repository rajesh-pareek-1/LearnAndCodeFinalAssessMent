using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Presentation.Helpers;

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
            if (!ConsoleInputHelper.Confirm("\nWould you like to delete a saved article?")) return;

            var articleId = ConsoleInputHelper.ReadPositiveInt("Enter the Article ID to delete: ");

            if (!savedArticles.Any(article => article.Id == articleId))
                throw new UserInputException($"No saved article found with ID {articleId}.");

            var isDeleted = await _savedArticleService.DeleteSavedArticleAsync(articleId);

            if (isDeleted)
                ConsoleOutputHelper.PrintSuccess("Article deleted successfully.");
            else
                ConsoleOutputHelper.PrintError("Failed to delete the article.");
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
            ConsoleOutputHelper.PrintError($"Input Error: {ex.Message}");
        }
        catch (ValidationException ex)
        {
            ConsoleOutputHelper.PrintError($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            ConsoleOutputHelper.PrintError($"Something went wrong: {ex.Message}");
        }
    }
}
