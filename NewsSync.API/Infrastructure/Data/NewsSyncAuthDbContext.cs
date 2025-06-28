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

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = RoleConstants.AdminRoleId,
                    ConcurrencyStamp = RoleConstants.AdminRoleId,
                    Name = RoleConstants.AdminRoleName,
                    NormalizedName = RoleConstants.AdminRoleName.ToUpper()
                },
                new IdentityRole
                {
                    Id = RoleConstants.UserRoleId,
                    ConcurrencyStamp = RoleConstants.UserRoleId,
                    Name = RoleConstants.UserRoleName,
                    NormalizedName = RoleConstants.UserRoleName.ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
