using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly NewsSyncNewsDbContext db;

    public AdminRepository(NewsSyncNewsDbContext db)
    {
        this.db = db;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await db.Categories.AddAsync(category);
    }

    public async Task<ServerStatusDto> GetServerStatusAsync()
    {
        var latest = await db.ServerDetails
            .OrderByDescending(s => s.LastAccess)
            .FirstOrDefaultAsync();

        if (latest == null)
        {
            return new ServerStatusDto
            {
                Uptime = TimeSpan.Zero,
                LastAccessed = DateTime.MinValue
            };
        }

        var uptime = DateTime.UtcNow - latest.LastAccess;

        return new ServerStatusDto
        {
            Uptime = uptime,
            LastAccessed = latest.LastAccess
        };
    }

    public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
    {
        var servers = await db.ServerDetails
            .OrderByDescending(s => s.LastAccess)
            .ToListAsync();

        return servers.Select(s => new ServerDetailsDto
        {
            ServerName = s.ServerName,
            ApiKey = s.ApiKey,
        }).ToList();
    }

    public Task<ServerDetail?> GetServerByIdAsync(int serverId)
    {
        return db.ServerDetails.FirstOrDefaultAsync(s => s.Id == serverId);
    }

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
