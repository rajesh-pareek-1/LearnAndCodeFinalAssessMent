using Microsoft.AspNetCore.Identity;

namespace NewsSync.API.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}

