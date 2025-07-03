namespace NewsSyncClient.Core.Interfaces.Api;

public interface IApiClient
{
    Task<T> GetAsync<T>(string uri);
    Task<T> PostAsync<TInput, T>(string uri, TInput data);
    Task<T> PutAsync<TInput, T>(string uri, TInput data);
    Task<bool> PostAsync<TInput>(string uri, TInput data);
    Task<bool> PutAsync<TInput>(string uri, TInput data);
    Task<bool> DeleteAsync(string uri);
}
