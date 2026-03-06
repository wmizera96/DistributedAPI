using DistributedAPI.CommonTools.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;

namespace DistributedAPI.CommonTools.Extensions;

public static class ServiceCollectionExtensions
{
    private const string AzureAdKey = "AzureAd";
    private const string AzureAdTenantIdKey = "AzureAd:TenantId";
    private const string AzureAdClientIdKey = "AzureAd:ClientId";
    
    
    public static IServiceCollection AddCommonApi(this IServiceCollection services, IConfiguration configuration, Action<CommonApiServiceCollectionOptions>? configure = null)
    {
        var commonOptions = new CommonApiServiceCollectionOptions();
        configure?.Invoke(commonOptions);
        
        var tenantId = configuration.GetValue<string>(AzureAdTenantIdKey);
        var clientId = configuration.GetValue<string>(AzureAdClientIdKey);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        { 
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),
                        AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"),
                        Scopes = new Dictionary<string, string>
                        {
                            { $"api://{clientId}/access_as_user", "Access API" }
                        }
                    }
                }
            });
            
            options.AddSecurityRequirement(document => new() { [new OpenApiSecuritySchemeReference("oauth2", document)] = [ $"api://{clientId}/access_as_user" ] });
        });

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