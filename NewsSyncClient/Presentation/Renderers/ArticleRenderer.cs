using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Renderers;

public class ArticleRenderer : IArticleRenderer
{
    public void Render(List<ArticleDto> articles)
    {
        Console.Clear();
        ConsoleOutputHelper.PrintHeader("Headlines");

        foreach (var article in articles)
        {
            ConsoleOutputHelper.PrintInfo($"ID          : {article.Id}");
            ConsoleOutputHelper.PrintInfo($"Headline    : {article.Headline}");
            ConsoleOutputHelper.PrintInfo($"Published   : {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
            ConsoleOutputHelper.PrintInfo($"Source      : {article.Url}");
            ConsoleOutputHelper.PrintInfo($"Description : {Truncate(article.Description, 160)}");
            ConsoleOutputHelper.PrintInfo($"Liked       : {(article.IsLiked ? "Yes" : "No")}   Disliked: {(article.IsDisliked ? "Yes" : "No")}");
            ConsoleOutputHelper.PrintInfo($"Category    : {article.CategoryName}");
            ConsoleOutputHelper.PrintDivider();
        }
    }

    private string Truncate(string? text, int maxLength) =>
        string.IsNullOrWhiteSpace(text) ? "N/A" : text.Length <= maxLength ? text : text[..maxLength] + "...";
}
