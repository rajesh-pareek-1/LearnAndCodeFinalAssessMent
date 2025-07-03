using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly NewsSyncNewsDbContext db;

        public ServerRepository(NewsSyncNewsDbContext db)
        {
            this.db = db;
        }

        public async Task<List<ServerStatusDto>> GetServerStatusAsync()
        {
            var servers = await db.ServerDetails
                .OrderByDescending(s => s.LastAccess)
                .ToListAsync();

            return servers.Select(s => new ServerStatusDto
            {
                Uptime = DateTime.UtcNow - s.LastAccess,
                LastAccessed = s.LastAccess
            }).ToList();
        }

        public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
        {
            var servers = await db.ServerDetails
                .OrderByDescending(s => s.LastAccess)
                .ToListAsync();

            return servers.Select(s => new ServerDetailsDto
            {
                Id = s.Id,
                ServerName = s.ServerName,
                ApiKey = s.ApiKey
            }).ToList();
        }

        public Task<ServerDetail?> GetByIdAsync(int id)
        {
            return db.ServerDetails.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task UpdateApiKeyAsync(ServerDetail server, string newKey)
        {
            if (string.IsNullOrWhiteSpace(newKey))
                throw new ArgumentException("API key cannot be empty.", nameof(newKey));

            server.ApiKey = newKey;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ServerDetail server)
        {
            db.ServerDetails.Update(server);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
