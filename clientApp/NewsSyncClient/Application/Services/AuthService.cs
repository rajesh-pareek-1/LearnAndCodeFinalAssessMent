using System.Net.Http.Json;
using System.Text.Json;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Auth;

namespace NewsSyncClient.Application.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientProvider _clientProvider;
    private readonly ISessionContext _session;

    public AuthService(IHttpClientProvider clientProvider, ISessionContext session)
    {
        _clientProvider = clientProvider;
        _session = session;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var payload = new { Username = email, Password = password };

        var response = await _clientProvider.Client.PostAsJsonAsync("api/auth/login", payload);
        if (!response.IsSuccessStatusCode) return false;

        var content = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (loginResponse == null || string.IsNullOrWhiteSpace(loginResponse.JwtToken)) return false;

        _clientProvider.SetJwtToken(loginResponse.JwtToken);
        _session.JwtToken = loginResponse.JwtToken;
        _session.UserId = loginResponse.UserId;
        _session.Email = email;
        _session.Role = loginResponse.Role;

        return true;
    }
}
