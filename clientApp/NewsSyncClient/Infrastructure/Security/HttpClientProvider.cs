using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using NewsSyncClient.Core.Interfaces.Api;
namespace NewsSyncClient.Infrastructure.Security;

public class HttpClientProvider : IHttpClientProvider
{
    private readonly HttpClient _httpClient;

    public HttpClientProvider(IConfiguration config)
    {
        var baseUrl = config["ApiBaseUrl"]
            ?? throw new InvalidOperationException("Missing configuration: ApiBaseUrl");

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public HttpClient Client => _httpClient;

    public void SetJwtToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
