using Microsoft.AspNetCore.Identity;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Repositories
{
    public interface ITokenRepository
    {
        public string CreateJWTToken(AppUser user, List<string> roles);
    }
}