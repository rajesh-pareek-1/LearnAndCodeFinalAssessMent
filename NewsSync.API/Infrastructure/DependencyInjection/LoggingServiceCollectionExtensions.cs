using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class LoggingServiceCollectionExtensions
    {
        public static void AddCustomSerilog(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Information()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
        }
    }
}
