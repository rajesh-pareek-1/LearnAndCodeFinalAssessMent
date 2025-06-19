using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Repositories
{
    public interface IAdminRepository
    {
        Task AddCategoryAsync(Category category);
        Task<ServerStatusDto> GetServerStatusAsync();
        Task<List<ServerDetailsDto>> GetServerDetailsAsync();
        Task<ServerDetail?> GetServerByIdAsync(int serverId);
        Task SaveChangesAsync();
    }

}