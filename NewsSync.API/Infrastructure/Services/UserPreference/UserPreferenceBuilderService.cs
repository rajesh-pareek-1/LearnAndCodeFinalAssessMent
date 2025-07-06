using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Services
{
    public class UserPreferenceBuilderService : IUserPreferenceBuilderService
    {
        private readonly IArticleReactionRepository reactionRepo;
        private readonly ISavedArticleRepository savedRepo;
        private readonly INotificationRepository notificationRepo;
        private readonly IUserPreferenceRepository preferenceRepo;
        private readonly ILogger<UserPreferenceBuilderService> logger;

        public UserPreferenceBuilderService(IArticleReactionRepository reactionRepo, ISavedArticleRepository savedRepo, INotificationRepository notificationRepo, IUserPreferenceRepository preferenceRepo, ILogger<UserPreferenceBuilderService> logger)
        {
            this.reactionRepo = reactionRepo;
            this.savedRepo = savedRepo;
            this.notificationRepo = notificationRepo;
            this.preferenceRepo = preferenceRepo;
            this.logger = logger;
        }

        public async Task UpdateAllUserPreferencesAsync()
        {
            logger.LogInformation("Starting user preference update cycle...");

            var userIds = await preferenceRepo.GetAllUserIdsAsync();
            foreach (var userId in userIds)
            {
                var preferences = await CalculatePreferencesAsync(userId);
                await preferenceRepo.UpsertUserPreferencesAsync(userId, preferences);
            }

            logger.LogInformation("Completed user preference update cycle.");
        }

        private async Task<List<UserPreference>> CalculatePreferencesAsync(string userId)
        {
            var preferences = new Dictionary<int, int>();

            var likedArticles = await reactionRepo.GetUserReactionsAsync(userId, isLiked: true);
            foreach (var likedArticle in likedArticles)
            {
                if (likedArticle.Article?.CategoryId is int catId)
                    AddWeight(preferences, catId, 3);
            }

            var savedArticles = await savedRepo.GetSavedArticlesByUserIdAsync(userId);
            foreach (var article in savedArticles)
            {
                if (article.CategoryId is int catId)
                    AddWeight(preferences, catId, 2);
            }

            var notificationConfigurations = await notificationRepo.GetNotificationSettingsAsync(userId);
            foreach (var notificationConfiguration in notificationConfigurations)
            {
                AddWeight(preferences, notificationConfiguration.CategoryId, 1);
            }

            var preferenceList = preferences.Select(userPreference => new UserPreference
            {
                UserId = userId,
                CategoryId = userPreference.Key,
                Weight = userPreference.Value
            }).ToList();

            logger.LogInformation("User {UserId} preferences: {Preferences}", userId,
                string.Join(", ", preferenceList.Select(userPreference => $"[Cat:{userPreference.CategoryId}, Weight:{userPreference.Weight}]")));

            return preferenceList;
        }

        private void AddWeight(Dictionary<int, int> dict, int categoryId, int weight)
        {
            if (dict.ContainsKey(categoryId))
                dict[categoryId] += weight;
            else
                dict[categoryId] = weight;
        }
    }
}