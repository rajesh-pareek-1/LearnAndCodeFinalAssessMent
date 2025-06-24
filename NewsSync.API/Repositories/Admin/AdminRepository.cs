using Microsoft.EntityFrameworkCore;
using NewsSync.API.Data;
using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly NewsSyncNewsDbContext _newsDbContext;

    public AdminRepository(NewsSyncNewsDbContext _newsDbContext)
    {
        this._newsDbContext = _newsDbContext;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _newsDbContext.Categories.AddAsync(category);
    }

    public async Task<List<ServerStatusDto>> GetServerStatusAsync()
    {
        var servers = await _newsDbContext.ServerDetails
            .OrderByDescending(s => s.LastAccess)
            .ToListAsync();

        return [.. servers.Select(s => new ServerStatusDto
        {
            Uptime = DateTime.UtcNow - s.LastAccess,
            LastAccessed = s.LastAccess
        })];
    }

    public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
    {
        var servers = await _newsDbContext.ServerDetails
            .OrderByDescending(s => s.LastAccess)
            .ToListAsync();

        return servers.Select(s => new ServerDetailsDto
        {
            Id = s.Id,
            ServerName = s.ServerName,
            ApiKey = s.ApiKey,
        }).ToList();
    }

    public Task<ServerDetail?> GetServerByIdAsync(int serverId)
    {
        return _newsDbContext.ServerDetails.FirstOrDefaultAsync(s => s.Id == serverId);
    }

    public Task SaveChangesAsync() => _newsDbContext.SaveChangesAsync();

    public async Task<Article?> GetArticleByIdAsync(int articleId)
    {
        return await _newsDbContext.Articles.FindAsync(articleId);
    }

}
