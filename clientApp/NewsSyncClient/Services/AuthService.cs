using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NewsSyncConsoleClient.State;

namespace NewsSyncConsoleClient.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public string? LoggedInUserId { get; private set; }

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var requestPayload = new
        {
            Username = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("api/auth/login", requestPayload);
        var rawJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return false;

        var loginResponse = DeserializeLoginResponse(rawJson);
        if (loginResponse == null || string.IsNullOrWhiteSpace(loginResponse.JwtToken))
            return false;

        SetAuthorizationHeader(loginResponse.JwtToken);
        UpdateGlobalState(email, loginResponse);

        return true;
    }

    private static LoginResponseDto? DeserializeLoginResponse(string rawJson)
    {
        try
        {
            return JsonSerializer.Deserialize<LoginResponseDto>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private void SetAuthorizationHeader(string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }

    private void UpdateGlobalState(string email, LoginResponseDto loginResponse)
    {
        GlobalAppState.JwtToken = loginResponse.JwtToken;
        GlobalAppState.UserId = loginResponse.UserId;
        GlobalAppState.UserRole = loginResponse.Role;
        GlobalAppState.Email = email;
        LoggedInUserId = loginResponse.UserId;
    }

    private class LoginResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
