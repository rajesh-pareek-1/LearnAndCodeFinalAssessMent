using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Presentation.Helpers;

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
        try
        {
            if (!ConsoleInputHelper.Confirm("\nDo you want to save an article?")) return;

            var articleId = ConsoleInputHelper.ReadPositiveInt("Enter Article ID to save: ");

            if (!articles.Any(a => a.Id == articleId))
                throw new UserInputException($"No article found with ID: {articleId}");

            await _savedService.SaveArticleAsync(articleId);
            ConsoleOutputHelper.PrintSuccess("Article saved successfully.");
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
            ConsoleOutputHelper.PrintError($"Unexpected error: {ex.Message}");
        }
    }
}
