using NewsSync.API.Application.DTOs;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface IServerRepository
    {
        Task<List<ServerStatusDto>> GetServerStatusAsync();
        Task<List<ServerDetailsDto>> GetServerDetailsAsync();
        Task<ServerDetail?> GetByIdAsync(int id);
        Task UpdateApiKeyAsync(ServerDetail server, string newKey);
        Task SaveChangesAsync();
        Task UpdateAsync(ServerDetail server);
    }
}
