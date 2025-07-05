using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface IUserPreferenceRepository
    {
        Task<List<int>> GetPreferredCategoryIdsAsync(string userId);
        Task UpdatePreferencesAsync(string userId, List<int> categoryIds);
        Task<List<string>> GetAllUserIdsAsync();
        Task UpsertUserPreferencesAsync(string userId, List<UserPreference> preferences);
    }

}