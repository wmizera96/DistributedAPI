using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.CommonTools.Test.API;

public class VersionStartup
{
    private IConfiguration Configuration { get; }
    
    public VersionStartup(IConfiguration configuration)
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
        app.UseCommonApi(Configuration);
    }
}