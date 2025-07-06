using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data
{
    public class NewsSyncAuthDbContext : IdentityDbContext<AppUser>
    {
        public NewsSyncAuthDbContext(DbContextOptions<NewsSyncAuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            var roles = new List<IdentityRole>
            {
                CreateRole(RoleConstants.AdminRoleId, RoleConstants.AdminRoleName),
                CreateRole(RoleConstants.UserRoleId, RoleConstants.UserRoleName)
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

        private static IdentityRole CreateRole(string id, string name)
        {
            return new IdentityRole
            {
                Id = id,
                ConcurrencyStamp = id,
                Name = name,
                NormalizedName = name.ToUpper()
            };
        }
    }
}
