using DistributedAPI.Books.Application;
using DistributedAPI.CommonTools.Extensions;

namespace DistributedAPI.Books.API;

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
            options.AddPolicy(BooksPolicy.Read);
            options.AddPolicy(BooksPolicy.Write);
        });
        
        services.AddScoped<IBooksService, BooksService>();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCommonApi(Configuration);
    }
}