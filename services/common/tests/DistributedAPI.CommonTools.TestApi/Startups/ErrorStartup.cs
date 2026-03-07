using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.CommonTools.Test.API.Startups;

public class ErrorStartup
{
    private IConfiguration Configuration { get; }
    
    public ErrorStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommonApi(Configuration);
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCommonApi(Configuration);
    }
}