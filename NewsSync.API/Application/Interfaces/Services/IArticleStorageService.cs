using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleStorageService
    {
        Task StoreArticlesAsync(ServerDetail server, List<Article> articles);
    }
}
