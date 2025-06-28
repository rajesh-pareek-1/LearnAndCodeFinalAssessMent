using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        public string CreateJWTToken(AppUser user, List<string> roles);
    }
}
