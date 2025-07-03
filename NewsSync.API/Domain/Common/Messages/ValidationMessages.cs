namespace NewsSync.API.Domain.Common.Messages
{
    public static class ValidationMessages
    {
        public const string UserIdRequired = "UserId is required.";
        public const string CategoryNotFound = "Category not found.";
        public const string UserIdAndCategoryRequired = "UserId and CategoryName are required.";
        public const string UpdateSuccess = "Updated successfully.";
        public const string DuplicateReport = "You’ve already reported this article.";
        public const string ArticleReported = "Article reported.";
        public const string InvalidInput = "Invalid input.";
        public const string ArticleAlreadySavedOrNotFound = "Article already saved or not found.";
        public const string ArticleSavedSuccess = "Article saved successfully.";
        public const string SavedArticleNotFound = "Saved article not found.";
        public const string SavedArticleDeletedSuccess = "Saved article deleted successfully.";
        public const string SearchQueryRequired = "Query parameter is required.";
        public const string ReportSubmitted = "Report submitted successfully.";
        public const string ReactionSubmitted = "Reaction submitted successfully.";
        public const string InvalidCategoryInput = "Category name is required.";
        public const string CategoryAdded = "Category added successfully.";
        public const string InvalidApiKey = "New API key is required.";
        public const string ServerApiKeyUpdated = "Server API key updated.";
        public const string ArticleBlocked = "Article has been blocked.";
        public const string ArticleUnblocked = "Article has been unblocked.";

        public const string InvalidRegistrationInput = "Username and password are required.";
        public const string InvalidLoginInput = "Login credentials are required.";
        public const string InvalidCredentials = "Invalid email or password.";
        public const string UserRegistered = "User registered successfully. Please log in.";
        public const string FailedToFetchNotifications = "Failed to retrieve notifications.";
        public const string FailedToFetchSettings = "Failed to retrieve notification settings.";
        public const string FailedToUpdateNotificationSetting = "Failed to update notification setting.";
        public const string FailedToSubmitReaction = "Failed to submit reaction.";
        public const string FailedToFetchReactions = "Failed to fetch user reactions.";
        public const string FailedToSubmitReport = "Failed to submit report.";
        public const string FailedToFetchSavedArticles = "Failed to fetch saved articles.";
        public const string FailedToSaveArticle = "Failed to save article.";
        public const string FailedToDeleteSavedArticle = "Failed to delete saved article.";
        public const string EmailSendFailed = "Failed to send email notification.";
        public const string InvalidEmailInput = "Email, subject, or body is invalid.";
        public const string FailedToFetchArticles = "Failed to fetch articles from external news sources.";
        public const string NoAdapterFoundForServer = "No adapter configured for the given server.";
        public const string NoArticlesFetched = "No articles were fetched from the server.";
        public const string FailedToSaveFetchedArticles = "Error occurred while saving fetched articles to the database.";
        public const string FailedToNotifyUsers = "Error occurred while sending notifications to users.";
        public const string NotificationSent = "Notification sent successfully.";

    }
}
