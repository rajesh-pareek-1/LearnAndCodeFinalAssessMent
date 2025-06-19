// Services/AdminService.cs
using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;
using NewsSync.API.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository repo;

    public AdminService(IAdminRepository repo)
    {
        this.repo = repo;
    }

    public async Task AddCategoryAsync(CategoryCreateRequestDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        await repo.AddCategoryAsync(category);
        await repo.SaveChangesAsync();
    }

    public Task<ServerStatusDto> GetServerStatusAsync()
        => repo.GetServerStatusAsync();

    public Task<List<ServerDetailsDto>> GetServerDetailsAsync()
        => repo.GetServerDetailsAsync();

    public async Task UpdateServerApiKeyAsync(int serverId, string newApiKey)
    {
        var server = await repo.GetServerByIdAsync(serverId);
        if (server == null) throw new Exception("Server not found");
        server.ApiKey = newApiKey;
        await repo.SaveChangesAsync();
    }
}
