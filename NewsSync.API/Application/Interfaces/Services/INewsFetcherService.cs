using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface INewsFetcherService
    {
        Task<List<Article>> FetchArticlesAsync(ServerDetail server);
    }
}