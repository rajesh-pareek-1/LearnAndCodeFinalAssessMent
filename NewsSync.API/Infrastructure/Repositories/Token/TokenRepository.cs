using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration config;

        public TokenRepository(IConfiguration config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string CreateJWTToken(AppUser user, List<string> roles)
        {
            ValidateInputs(user, roles);

            var claims = BuildClaims(user.Email!, roles);
            var signingCredentials = GetSigningCredentials();

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void ValidateInputs(AppUser user, List<string> roles)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("User email cannot be null or empty.", nameof(user.Email));

            if (roles == null)
                throw new ArgumentNullException(nameof(roles));
        }

        private List<Claim> BuildClaims(string email, List<string> roles)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };

            foreach (var role in roles.Where(r => !string.IsNullOrWhiteSpace(r)))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtKey = config["Jwt:Key"];
            var key = string.IsNullOrWhiteSpace(jwtKey)
                ? throw new InvalidOperationException("JWT secret key is missing in configuration.")
                : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
