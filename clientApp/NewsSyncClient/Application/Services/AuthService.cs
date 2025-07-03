using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApiClient _apiClient;
    private readonly IHttpClientProvider _clientProvider;
    private readonly ISessionContext _session;

    public AuthService(IApiClient apiClient, IHttpClientProvider clientProvider, ISessionContext session)
    {
        _apiClient = apiClient;
        _clientProvider = clientProvider;
        _session = session;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var payload = new { Username = email, Password = password };

        LoginResponseDto loginResponse;
        try
        {
            loginResponse = await _apiClient.PostAsync<object, LoginResponseDto>("/api/auth/login", payload);
        }
        catch
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(loginResponse.JwtToken)) return false;

        _clientProvider.SetJwtToken(loginResponse.JwtToken);
        
        _session.JwtToken = loginResponse.JwtToken;
        _session.UserId = loginResponse.UserId;
        _session.Email = email;
        _session.Role = loginResponse.Role;

        return true;
    }
}
