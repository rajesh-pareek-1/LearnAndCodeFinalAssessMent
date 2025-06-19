using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NewsSync.API.Models.Domain;

public static class IdentityDataSeeder
{
    public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
    {
        Console.WriteLine("✅ Starting Identity Seeding...");

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. Ensure roles exist
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seed Admin
        var adminEmail = "admin_rajesh@newssync.com";
        var adminPassword = "Admin_Rajesh@123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            Console.WriteLine("✅ Creating admin...");

            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 3. Seed normal users
        var normalUsers = new List<(string email, string password)>
        {
            ("aditya@newssync.com", "User@123"),
            ("shivesh@newssync.com", "User@123"),
            ("akshatbhai@newssync.com", "User@123")
        };

        foreach (var (email, password) in normalUsers)
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
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
