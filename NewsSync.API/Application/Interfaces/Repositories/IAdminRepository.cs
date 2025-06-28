using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task AddCategoryAsync(Category category);
        Task<List<ServerStatusDto>> GetServerStatusAsync();
        Task<List<ServerDetailsDto>> GetServerDetailsAsync();
        Task<ServerDetail?> GetServerByIdAsync(int serverId);
        Task<Article?> GetArticleByIdAsync(int articleId);
        Task SaveChangesAsync();
    }

}