using System.Net.Http.Json;
using DistributedAPI.CommonTools;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedAPI.TestTools;

public class ApiCaller<TStartup> where TStartup : class
{
    private readonly ApiFactory<TStartup> _apiFactory;
    private Action<IServiceCollection> _configureServices = _ => { };

    public ApiCaller(ApiFactory<TStartup> apiFactory)
    {
        _apiFactory = apiFactory;
    }

    public void ConfigureCustomServices(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;
    }

    public async Task<HttpResponseMessage> GetAsync(string url, IEnumerable<BasePolicy> permissions, bool isAuthenticated = true)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, url);
        
        return await SendRequestAsync(message, permissions, isAuthenticated);
    }
    
    public async Task<HttpResponseMessage> PostAsync<T>(string url, T data, IEnumerable<BasePolicy> permissions, bool isAuthenticated = true)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, url);
        message.Content = JsonContent.Create(data);
        return await SendRequestAsync(message, permissions, isAuthenticated);
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage message, IEnumerable<BasePolicy> permissions, bool isAuthenticated)
    {
        _apiFactory.ConfigureCustomServices(_configureServices);
        using var client = isAuthenticated ? _apiFactory.CreateAuthenticatedHttpClient(permissions) :  _apiFactory.CreateUnauthenticatedHttpClient();

        return await client.SendAsync(message);
    }
}
