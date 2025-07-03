using System.Net.Http.Json;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Api;


namespace NewsSyncClient.Infrastructure.Api;

public class ApiClient : IApiClient
{
    private readonly HttpClient _client;

    public ApiClient(IHttpClientProvider provider)
    {
        _client = provider.Client;
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        var response = await _client.GetAsync(uri);
        return await HandleResponse<T>(response, $"GET {uri} failed");
    }

    public async Task<T> PostAsync<TInput, T>(string uri, TInput data)
    {
        var response = await _client.PostAsJsonAsync(uri, data);
        return await HandleResponse<T>(response, $"POST {uri} failed");
    }

    public async Task<T> PutAsync<TInput, T>(string uri, TInput data)
    {
        var response = await _client.PutAsJsonAsync(uri, data);
        return await HandleResponse<T>(response, $"PUT {uri} failed");
    }

    public async Task<bool> PostAsync<TInput>(string uri, TInput data)
    {
        var response = await _client.PostAsJsonAsync(uri, data);
        return await HandleResponse(response, $"POST {uri} failed");
    }

    public async Task<bool> PutAsync<TInput>(string uri, TInput data)
    {
        var response = await _client.PutAsJsonAsync(uri, data);
        return await HandleResponse(response, $"PUT {uri} failed");
    }

    public async Task<bool> DeleteAsync(string uri)
    {
        var response = await _client.DeleteAsync(uri);
        return await HandleResponse(response, $"DELETE {uri} failed");
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage response, string errorMessage)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new ApiException($"{errorMessage}: {response.StatusCode} | {body}", (int)response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<T>() ?? throw new ApiException("Empty response", (int)response.StatusCode);
    }

    private async Task<bool> HandleResponse(HttpResponseMessage response, string errorMessage)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new ApiException($"{errorMessage}: {response.StatusCode} | {body}", (int)response.StatusCode);
        }

        return true;
    }
}
