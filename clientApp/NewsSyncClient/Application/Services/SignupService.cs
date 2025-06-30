using System.Net.Http.Json;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Application.Services;

public class SignupService : ISignupService
{
    private readonly IHttpClientProvider _clientProvider;

    public SignupService(IHttpClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(SignupRequestDto dto)
    {
        try
        {
            var response = await _clientProvider.Client.PostAsJsonAsync("api/auth/register", dto);

            if (response.IsSuccessStatusCode)
                return (true, "Signup successful! Please proceed to login.");

            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Signup failed: {error}");
        }
        catch (Exception ex)
        {
            return (false, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
