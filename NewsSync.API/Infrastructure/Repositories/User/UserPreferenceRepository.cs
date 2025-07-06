using Microsoft.EntityFrameworkCore;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;
        private readonly NewsSyncAuthDbContext authDb;

        public UserPreferenceRepository(NewsSyncNewsDbContext newsDb, NewsSyncAuthDbContext authDb)
        {
            this.newsDb = newsDb;
            this.authDb = authDb;
        }

        public async Task<List<int>> GetPreferredCategoryIdsAsync(string userId)
        {
            return await newsDb.UserPreferences
                .Where(userPreference => userPreference.UserId == userId)
                .OrderByDescending(userPreference => userPreference.Weight)
                .Select(userPreference => userPreference.CategoryId)
                .ToListAsync();
        }

        public async Task UpdatePreferencesAsync(string userId, List<int> categoryIds)
        {
            var existing = await newsDb.UserPreferences
                .Where(userPreference => userPreference.UserId == userId)
                .ToListAsync();

            newsDb.UserPreferences.RemoveRange(existing);

            var newPrefs = categoryIds.Select(categoryId => new UserPreference
            {
                UserId = userId,
                CategoryId = categoryId,
                Weight = 1
            }).ToList();

            await newsDb.UserPreferences.AddRangeAsync(newPrefs);
            await newsDb.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllUserIdsAsync()
        {
            return await authDb.Users
                .Select(user => user.Id)
                .Distinct()
                .ToListAsync();
        }

        public async Task UpsertUserPreferencesAsync(string userId, List<UserPreference> newPreferences)
        {
            var existing = await newsDb.UserPreferences
                .Where(userPreference => userPreference.UserId == userId)
                .ToListAsync();

            newsDb.UserPreferences.RemoveRange(existing);
            await newsDb.UserPreferences.AddRangeAsync(newPreferences);
            await newsDb.SaveChangesAsync();
        }
    }
}
