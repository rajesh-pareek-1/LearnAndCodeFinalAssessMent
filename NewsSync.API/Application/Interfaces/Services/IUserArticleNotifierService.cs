using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IUserArticleNotifierService
    {
        Task NotifyUsersAsync(List<Article> articles);
    }
}