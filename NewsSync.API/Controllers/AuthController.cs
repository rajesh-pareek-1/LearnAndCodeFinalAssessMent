using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.Contants;
using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;

namespace NewsSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<AppUser> _userManager, ITokenRepository _tokenRepository)
        {
            this._userManager = _userManager;
            this._tokenRepository = _tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var appUser = new AppUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var result = await _userManager.CreateAsync(appUser, registerRequestDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(appUser, RoleNames.User);

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok("User was registered. Please log in.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                return BadRequest("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenRepository.CreateJWTToken(user, roles.ToList());

            return Ok(new LoginResponseDto
            {
                JwtToken = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }
    }
}
