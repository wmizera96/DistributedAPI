using DistributedAPI.CommonTools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistributedAPI.TestTools;


public class ApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private Action<IServiceCollection> _configureServices;
    private IEnumerable<BasePolicy> _policies;
    
    public void ConfigureCustomServices(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;
    }

    public HttpClient CreateAuthenticatedHttpClient(IEnumerable<BasePolicy> policies)
    {
        _policies = policies;
        
        return CreateClient();
    }

    protected override IHostBuilder? CreateHostBuilder()
    {
        return ApiHost.CreateHostBuilder<TStartup>([]);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseStartup<TStartup>();
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.AddAuthorization();
            
            services.AddScoped<TestUserContext>(_ => new TestUserContext(_policies));
        });
        
        builder.ConfigureServices(_configureServices);
    }
}