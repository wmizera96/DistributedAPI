using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.CommonTools.Test.API.Startups;

public class DefaultHealthCheckStartup
{
    private IConfiguration Configuration { get; }
    
    public DefaultHealthCheckStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommonApi(Configuration, options =>
        {
            options.AddPolicy(TestPolicy.Read);
            options.AddPolicy(TestPolicy.Write);
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCommonApi(Configuration, options =>
        {
            // no custom health-checks defined
        });
    }
}