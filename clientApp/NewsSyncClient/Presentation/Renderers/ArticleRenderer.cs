using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Renderers;

public class ArticleRenderer : IArticleRenderer
{
    public void Render(List<ArticleDto> articles)
    {
        Console.Clear();
        Console.WriteLine("===== Headlines =====\n");

        foreach (var article in articles)
        {
            Console.WriteLine($"ID          : {article.Id}");
            Console.WriteLine($"Headline    : {article.Headline}");
            Console.WriteLine($"Published   : {article.PublishedDate:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine($"Author      : {article.AuthorName ?? "N/A"}");
            Console.WriteLine($"Source      : {(string.IsNullOrWhiteSpace(article.Source) ? "N/A" : article.Source)}");
            Console.WriteLine($"URL         : {article.Url}");
            Console.WriteLine($"Image       : {(string.IsNullOrWhiteSpace(article.ImageUrl) ? "N/A" : article.ImageUrl)}");
            Console.WriteLine($"Language    : {(string.IsNullOrWhiteSpace(article.Language) ? "N/A" : article.Language)}");
            Console.WriteLine($"Description : {Truncate(article.Description, 160)}");
            Console.WriteLine($"Liked       : {(article.IsLiked ? "Yes" : "No")}   Disliked: {(article.IsDisliked ? "Yes" : "No")}");
            Console.WriteLine(new string('-', 80));
        }
    }

    private string Truncate(string? text, int maxLength) =>
        string.IsNullOrWhiteSpace(text) ? "N/A" : text.Length <= maxLength ? text : text[..maxLength] + "...";
}
