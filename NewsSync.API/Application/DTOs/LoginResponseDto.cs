namespace NewsSync.API.Application.DTOs
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
