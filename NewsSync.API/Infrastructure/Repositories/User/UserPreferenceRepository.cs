using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly NewsSyncNewsDbContext db;
        private readonly NewsSyncAuthDbContext authDb;

        public UserPreferenceRepository(NewsSyncNewsDbContext db, NewsSyncAuthDbContext authDb)
        {
            this.db = db;
            this.authDb = authDb;
        }

        public async Task<List<int>> GetPreferredCategoryIdsAsync(string userId)
        {
            return await db.UserPreferences
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Weight)
                .Select(p => p.CategoryId)
                .ToListAsync();
        }

        public async Task UpdatePreferencesAsync(string userId, List<int> categoryIds)
        {
            var existing = await db.UserPreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            db.UserPreferences.RemoveRange(existing);

            var newPrefs = categoryIds.Select(cId => new UserPreference
            {
                UserId = userId,
                CategoryId = cId,
                Weight = 1
            }).ToList();

            await db.UserPreferences.AddRangeAsync(newPrefs);
            await db.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllUserIdsAsync()
        {
            return await authDb.Users
                .Select(u => u.Id)
                .Distinct()
                .ToListAsync();
        }

        public async Task UpsertUserPreferencesAsync(string userId, List<UserPreference> newPreferences)
        {
            var existing = await db.UserPreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            db.UserPreferences.RemoveRange(existing);
            await db.UserPreferences.AddRangeAsync(newPreferences);
            await db.SaveChangesAsync();
        }

    }

}