using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Core.Interfaces;

public interface ISignupService
{
    Task<(bool Success, string Message)> RegisterAsync(SignupRequestDto dto);
}
