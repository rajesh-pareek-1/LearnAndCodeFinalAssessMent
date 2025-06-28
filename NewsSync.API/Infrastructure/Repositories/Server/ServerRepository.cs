using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly NewsSyncNewsDbContext context;

        public ServerRepository(NewsSyncNewsDbContext context)
        {
            this.context = context;
        }

        public async Task<List<ServerStatusDto>> GetServerStatusAsync()
        {
            var servers = await context.ServerDetails.OrderByDescending(s => s.LastAccess).ToListAsync();
            return servers.Select(s => new ServerStatusDto
            {
                Uptime = DateTime.UtcNow - s.LastAccess,
                LastAccessed = s.LastAccess
            }).ToList();
        }

        public async Task<List<ServerDetailsDto>> GetServerDetailsAsync()
        {
            var servers = await context.ServerDetails.OrderByDescending(s => s.LastAccess).ToListAsync();
            return servers.Select(s => new ServerDetailsDto
            {
                Id = s.Id,
                ServerName = s.ServerName,
                ApiKey = s.ApiKey
            }).ToList();
        }

        public Task<ServerDetail?> GetByIdAsync(int id)
            => context.ServerDetails.FirstOrDefaultAsync(s => s.Id == id);

        public Task UpdateApiKeyAsync(ServerDetail server, string newKey)
        {
            server.ApiKey = newKey;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ServerDetail server)
        {
            context.ServerDetails.Update(server);
            return Task.CompletedTask;
        }
        
        public Task SaveChangesAsync() => context.SaveChangesAsync();
    }

}