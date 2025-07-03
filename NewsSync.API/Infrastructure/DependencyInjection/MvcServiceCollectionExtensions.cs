using System.Text.Json.Serialization;
using NewsSync.API.Filters;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class Controllers
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            return services;
        }
    }
}
