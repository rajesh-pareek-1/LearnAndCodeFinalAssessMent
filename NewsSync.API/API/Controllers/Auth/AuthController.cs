using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Common.Messages;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(ValidationMessages.InvalidRegistrationInput);

            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Username
            };

            var creationResult = await userManager.CreateAsync(user, dto.Password);
            if (!creationResult.Succeeded)
                return BadRequest(creationResult.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, RoleNames.User);
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(ValidationMessages.UserRegistered);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(ValidationMessages.InvalidLoginInput);

            var user = await userManager.FindByEmailAsync(dto.Username);
            if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
                return BadRequest(ValidationMessages.InvalidCredentials);

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenRepository.CreateJWTToken(user, roles.ToList());

            var response = new LoginResponseDto
            {
                JwtToken = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault() ?? RoleNames.User
            };

            return Ok(response);
        }
    }
}
