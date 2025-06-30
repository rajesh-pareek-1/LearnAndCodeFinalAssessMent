using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;

public interface IArticleInteractionService
{
    Task<List<ArticleDto>> FetchHeadlinesAsync(DateTime from, DateTime to, string? category);
    Task<List<CategoryDto>> FetchCategoriesAsync();
    Task SaveArticleAsync(int articleId);
    Task ReactToArticleAsync(int articleId, bool isLiked);
    Task ReportArticleAsync(int articleId, string? reason);
    Task<List<ArticleDto>> GetUserReactionsAsync(bool liked);
}
