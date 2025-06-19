using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;
using NewsSync.API.Models.Domain; // Make sure AppUser is defined here

namespace NewsSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var appUser = new AppUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var result = await userManager.CreateAsync(appUser, registerRequestDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 👇 Automatically assign the "User" role (don't accept from client)
            var roleResult = await userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok("User was registered. Please log in.");
        }

        // POST: api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                return BadRequest("Invalid email or password");

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenRepository.CreateJWTToken(user, roles.ToList());

            return Ok(new LoginResponseDto
            {
                JwtToken = token,
                UserId = user.Id
            });
        }
    }
}
