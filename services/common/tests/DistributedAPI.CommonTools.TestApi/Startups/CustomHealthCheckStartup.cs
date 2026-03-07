using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.CommonTools.Test.API.Startups;

public class CustomHealthCheckStartup
{
    private IConfiguration Configuration { get; }
    
    public CustomHealthCheckStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommonApi(Configuration);
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCommonApi(Configuration, options =>
        {
            options.UseReadinessHealthCheck(async context => await context.RequestServices.GetRequiredService<ITestHealthCheckService>().RunAsync());
            options.UseLivenessHealthCheck(async context => await context.RequestServices.GetRequiredService<ITestHealthCheckService>().RunAsync());
        });
    }
}