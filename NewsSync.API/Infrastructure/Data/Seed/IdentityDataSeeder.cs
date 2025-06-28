using Microsoft.AspNetCore.Identity;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data.Seed
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("IdentitySeeder");

            logger.LogInformation("Seeding identity roles and users...");

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { RoleNames.Admin, RoleNames.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role created: {Role}", role);
                }
            }

            if (await userManager.FindByEmailAsync(SeedUserConfig.AdminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    UserName = SeedUserConfig.AdminEmail,
                    Email = SeedUserConfig.AdminEmail
                };

                var result = await userManager.CreateAsync(adminUser, SeedUserConfig.AdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
                    logger.LogInformation("Admin user created: {Email}", SeedUserConfig.AdminEmail);
                }
                else
                {
                    logger.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            foreach (var (email, password) in SeedUserConfig.DefaultUsers)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser
                    {
                        UserName = email,
                        Email = email
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RoleNames.User);
                        logger.LogInformation("User created: {Email}", email);
                    }
                    else
                    {
                        logger.LogWarning("Failed to create user {Email}: {Errors}", email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }

}