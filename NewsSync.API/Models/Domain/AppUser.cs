using Microsoft.AspNetCore.Identity;

namespace NewsSync.API.Models.Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}

