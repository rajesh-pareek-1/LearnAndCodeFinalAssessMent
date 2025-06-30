using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Prompts;

public class SavedArticlePrompt : ISavedArticlePrompt
{
    private readonly ISavedArticleService _savedArticleService;

    public SavedArticlePrompt(ISavedArticleService savedArticleService) => _savedArticleService = savedArticleService;

    public async Task ShowAsync(List<ArticleDto> savedArticles)
    {
        Console.Write("\nWould you like to delete a saved article? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        Console.Write("Enter the Article ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var articleId) || !savedArticles.Any(article => article.Id == articleId))
        {
            Console.WriteLine("Invalid article ID.");
            return;
        }

        var isDeleted = await _savedArticleService.DeleteSavedArticleAsync(articleId);
        Console.WriteLine(isDeleted ? "Article deleted successfully." : "Failed to delete the article.");
    }
}
