using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.CommonTools.Test.API;

public class Startup
{
    private IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
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
            options.UseReadinessHealthCheck(async context => await context.RequestServices.GetRequiredService<ITestHealthCheckService>().RunAsync());
            options.UseLivenessHealthCheck(async context => await context.RequestServices.GetRequiredService<ITestHealthCheckService>().RunAsync());
        });
    }
}