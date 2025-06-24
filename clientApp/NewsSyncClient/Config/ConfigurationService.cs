using Microsoft.Extensions.Configuration;

namespace NewsSyncConsoleClient.Services;

public static class ConfigurationService
{
    private static IConfigurationRoot? _config;

    static ConfigurationService()
    {
        _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    }

    public static string GetApiBaseUrl()
    {
        return _config!["ApiBaseUrl"]!;
    }
}
