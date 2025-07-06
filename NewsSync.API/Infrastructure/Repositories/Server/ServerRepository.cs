using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public ServerRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task<List<ServerStatusDto>> GetServerStatusAsync()
        {
            var servers = await newsDb.ServerDetails
                .OrderByDescending(server => server.LastAccess)
                .ToListAsync();

            return [.. servers.Select(server => new ServerStatusDto
            {
                Uptime = DateTime.UtcNow - server.LastAccess,
                LastAccessed = server.LastAccess
            })];
        }

        public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
        {
            var servers = await newsDb.ServerDetails
                .OrderByDescending(server => server.LastAccess)
                .ToListAsync();

            return [.. servers.Select(server => new ServerDetailsDto
            {
                Id = server.Id,
                ServerName = server.ServerName,
                ApiKey = server.ApiKey
            })];
        }

        public Task<ServerDetail?> GetByIdAsync(int serverId)
        {
            return newsDb.ServerDetails.FirstOrDefaultAsync(server => server.Id == serverId);
        }

        public Task UpdateApiKeyAsync(ServerDetail server, string newApiKey)
        {
            if (string.IsNullOrWhiteSpace(newApiKey))
                throw new ArgumentException("API key cannot be empty.", nameof(newApiKey));

            server.ApiKey = newApiKey;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ServerDetail server)
        {
            newsDb.ServerDetails.Update(server);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return newsDb.SaveChangesAsync();
        }
    }
}
