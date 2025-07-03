using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.Prompts;
public interface IArticleActionPrompt
{
    Task ShowAsync(List<ArticleDto> articles);
}
