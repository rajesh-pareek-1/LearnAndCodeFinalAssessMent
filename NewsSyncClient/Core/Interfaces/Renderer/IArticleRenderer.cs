using NewsSyncClient.Core.Models.Articles;
namespace NewsSyncClient.Core.Interfaces.Renderer;

public interface IArticleRenderer
{
    void Render(List<ArticleDto> articles);
}