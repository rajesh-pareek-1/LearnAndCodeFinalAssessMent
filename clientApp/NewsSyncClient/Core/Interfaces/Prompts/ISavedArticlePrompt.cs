using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.Prompts;

public interface ISavedArticlePrompt
{
    Task ShowAsync(List<ArticleDto> articles);
}
