using Microsoft.AspNetCore.Identity;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data.Seed
{
    public class IdentityDataSeeder
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;

        public IdentityDataSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task SeedAsync()
        {
            logger.LogInformation("Seeding identity roles and users...");
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedDefaultUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { RoleNames.Admin, RoleNames.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role created: {Role}", role);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (await userManager.FindByEmailAsync(SeedUserConfig.AdminEmail) != null)
                return;

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

        private async Task SeedDefaultUsersAsync()
        {
            foreach (var (email, password) in SeedUserConfig.DefaultUsers)
            {
                if (await userManager.FindByEmailAsync(email) != null)
                    continue;

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
