using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Presentation.Prompts;

public class ArticleActionPrompt : IArticleActionPrompt
{
    private readonly IArticleInteractionService _articleService;
    private readonly IArticleRenderer _renderer;

    public ArticleActionPrompt(IArticleInteractionService articleService, IArticleRenderer renderer)
    {
        _articleService = articleService;
        _renderer = renderer;
    }

    public async Task ShowAsync(List<ArticleDto> articles)
    {
        while (true)
        {
            Console.WriteLine("\nActions:");
            Console.WriteLine("1. Save");
            Console.WriteLine("2. Like");
            Console.WriteLine("3. Dislike");
            Console.WriteLine("4. Report");
            Console.WriteLine("5. Show Liked First");
            Console.WriteLine("6. Show Disliked First");
            Console.WriteLine("7. Back");
            Console.Write("Choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ExecuteForArticleAsync(articles, _articleService.SaveArticleAsync, "Saved");
                    break;
                case "2":
                    await ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, true), "Liked");
                    break;
                case "3":
                    await ExecuteForArticleAsync(articles, id => _articleService.ReactToArticleAsync(id, false), "Disliked");
                    break;
                case "4":
                    Console.Write("Reason: ");
                    var reason = Console.ReadLine();
                    await ExecuteForArticleAsync(articles, id => _articleService.ReportArticleAsync(id, reason), "Reported");
                    break;
                case "5":
                    var likedFirst = articles
                        .OrderByDescending(a => a.IsLiked)
                        .ToList();
                    _renderer.Render(likedFirst);
                    break;
                case "6":
                    var dislikedFirst = articles
                        .OrderByDescending(a => a.IsDisliked)
                        .ToList();
                    _renderer.Render(dislikedFirst);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid.");
                    break;
            }
        }
    }

    private async Task ExecuteForArticleAsync(List<ArticleDto> articles, Func<int, Task> action, string successMessage)
    {
        Console.Write("Enter Article ID: ");
        if (int.TryParse(Console.ReadLine(), out var id) && articles.Any(a => a.Id == id))
        {
            await action(id);
            Console.WriteLine(successMessage);
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }
}
