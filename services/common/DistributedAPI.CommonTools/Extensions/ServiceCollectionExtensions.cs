using DistributedAPI.CommonTools.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace DistributedAPI.CommonTools.Extensions;

public static class ServiceCollectionExtensions
{
    private const string AzureAdKey = "AzureAd";
    
    
    public static IServiceCollection AddCommonApi(this IServiceCollection services, IConfiguration configuration, Action<CommonApiServiceCollectionOptions>? configure = null)
    {
        var commonOptions = new CommonApiServiceCollectionOptions();
        configure?.Invoke(commonOptions);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, AzureAdKey);

        services.AddAuthorization(options =>
        {
            foreach (var policy in commonOptions.Policies)
            {
                options.AddPolicy(policy.Name, config => config.RequireRole(policy.RequiredRole));
            }
        });
        
        return services;
    }
}