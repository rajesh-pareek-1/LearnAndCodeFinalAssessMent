using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Prompts;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Core.Interfaces.Screens;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;
using NewsSyncClient.Infrastructure.Security;
using NewsSyncClient.Presentation.Prompts;
using NewsSyncClient.Presentation.Renderers;
using NewsSyncClient.Presentation.Screens;

namespace NewsSyncClient;

public static class ConsoleAppHost
{
    public static async Task RunAsync(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                      .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;
                var apiBaseUrl = config["ApiBaseUrl"]
                    ?? throw new InvalidOperationException("Missing configuration: ApiBaseUrl");

                // Register a singleton HttpClient for the entire app
                services.AddSingleton(new HttpClient
                {
                    BaseAddress = new Uri(apiBaseUrl)
                });

                // Core services
                services.AddSingleton<ISessionContext, SessionContext>();
                services.AddSingleton<IHttpClientProvider, HttpClientProvider>();

                // Auth flow
                services.AddSingleton<IAuthService, AuthService>();
                services.AddSingleton<ISignupService, SignupService>();
                services.AddSingleton<LoginScreen>();
                services.AddSingleton<SignupScreen>();
                services.AddSingleton<UserDashboardScreen>();

                // Article interaction
                services.AddSingleton<IArticleInteractionService, ArticleInteractionService>();
                services.AddSingleton<ISavedArticleService, SavedArticleService>();
                services.AddSingleton<IFetchSavedArticlesUseCase, FetchSavedArticlesUseCase>();
                services.AddSingleton<ISavedArticlePrompt, SavedArticlePrompt>();

                // Article use-cases and helpers
                services.AddSingleton<IFetchHeadlinesUseCase, FetchHeadlinesUseCase>();
                services.AddSingleton<IArticleRenderer, ArticleRenderer>();
                services.AddSingleton<IArticleActionPrompt, ArticleActionPrompt>();

                // News & article screens
                services.AddSingleton<IHeadlinesScreen, HeadlinesScreen>();
                services.AddSingleton<HeadlinesScreen>(); 
                services.AddSingleton<ISavedArticlesScreen, SavedArticlesScreen>();
                services.AddSingleton<SavedArticlesScreen>();

                // Notifications
                services.AddSingleton<INotificationService, NotificationService>();
                services.AddSingleton<INotificationsScreen, NotificationsScreen>();
                services.AddSingleton<NotificationsScreen>();

                // Admin flow
                services.AddSingleton<IAdminService, AdminService>();
                services.AddSingleton<IAdminRenderer, AdminRenderer>();
                services.AddSingleton<IAdminDashboardCoordinator, AdminDashboardCoordinator>();
                services.AddSingleton<IAdminDashboardScreen, AdminDashboardScreen>();
                services.AddSingleton<AdminDashboardScreen>();

                // Article search
                services.AddSingleton<ISearchArticleService, SearchArticleService>();
                services.AddSingleton<ISearchArticlesScreen, SearchArticlesScreen>();
                services.AddSingleton<SearchArticlesScreen>();
                services.AddSingleton<ISearchArticlesPrompt,SearchArticlesPrompt>();
                services.AddSingleton<ISearchArticlesUseCase, SearchArticlesUseCase>();

                // App flow
                services.AddSingleton<AppNavigator>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        var navigator = host.Services.GetRequiredService<AppNavigator>();
        await navigator.StartAsync();
    }
}
