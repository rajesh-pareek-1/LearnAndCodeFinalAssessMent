using Microsoft.AspNetCore.Identity;

namespace NewsSync.API.Repositories
{
    public interface ITokenRepository
    {
        public string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}