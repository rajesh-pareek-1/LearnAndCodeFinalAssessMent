using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class Controllers
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            return services;
        }
    }
}
