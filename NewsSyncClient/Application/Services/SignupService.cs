using NewsSyncClient.Core.Exceptions;
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
            ValidateSignupDto(dto);

            var result = await _apiClient.PostAsync("api/auth/register", dto);
            return (result, result ? "Signup successful! Please proceed to login." : "Signup failed.");
        }
        catch (ValidationException ex)
        {
            return (false, $"Validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"Signup failed: {ex.Message}");
        }
    }

    private void ValidateSignupDto(SignupRequestDto dto)
    {
        if (dto is null)
            throw new ValidationException("Signup data cannot be null.");

        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ValidationException("Username is required.");

        if (!dto.Username.Contains('@'))
            throw new ValidationException("Username must be a valid email.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ValidationException("Password is required.");

        if (dto.Password.Length < 6)
            throw new ValidationException("Password must be at least 6 characters.");
    }
}
