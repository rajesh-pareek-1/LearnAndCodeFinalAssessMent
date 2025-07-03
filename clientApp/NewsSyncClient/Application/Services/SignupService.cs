using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Application.Services;

public class SignupService : ISignupService
{
    private readonly IApiClient _apiClient;

    public SignupService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(SignupRequestDto dto)
    {
        try
        {
            var result = await _apiClient.PostAsync("api/auth/register", dto);
            return (result, result ? "Signup successful! Please proceed to login." : "Signup failed.");
        }
        catch (Exception ex)
        {
            return (false, $"Signup failed: {ex.Message}");
        }
    }
}
