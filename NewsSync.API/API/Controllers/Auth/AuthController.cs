using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Common.Messages;
using AutoMapper;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var user = mapper.Map<AppUser>(registerRequestDto);

            var creationResult = await userManager.CreateAsync(user, registerRequestDto.Password);
            if (!creationResult.Succeeded)
                return BadRequest(creationResult.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, RoleNames.User);
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok(ValidationMessages.UserRegistered);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user is null || !await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                return BadRequest(ValidationMessages.InvalidCredentials);

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenRepository.CreateJWTToken(user, [.. roles]);

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
