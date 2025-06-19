namespace NewsSync.API.Models.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}