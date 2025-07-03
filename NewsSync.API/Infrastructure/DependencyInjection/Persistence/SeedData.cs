using NewsSync.API.Infrastructure.Data.Seed;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class SeedData
    {
        public static async Task SeedInitialDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var seeder = services.GetRequiredService<IdentityDataSeeder>();
            await seeder.SeedAsync();
        }
    }
}
