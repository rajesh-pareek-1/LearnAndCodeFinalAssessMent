using Microsoft.Extensions.Configuration;
using NewsSyncClient.Screens;
using NewsSyncConsoleClient.Services;

namespace NewsSyncConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var apiBaseUrl = configuration["ApiBaseUrl"]
                ?? throw new InvalidOperationException("Missing configuration: ApiBaseUrl");

            using var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
            var authService = new AuthService(httpClient);

            await AppNavigator.RenderHomeScreenAsync(authService, httpClient);
        }

        private static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
