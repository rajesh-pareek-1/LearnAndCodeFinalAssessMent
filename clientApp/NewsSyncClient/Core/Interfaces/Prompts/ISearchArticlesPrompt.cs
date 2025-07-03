using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Core.Interfaces.Prompts;

public interface ISearchArticlesPrompt
{
    Task PromptToSaveAsync(List<ArticleDto> articles);
}
