using DistributedAPI.CommonTools.Extensions.Configuration;
using DistributedAPI.CommonTools.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DistributedAPI.CommonTools.Extensions;

public static class ApplicationBuilderExtensions
{
    internal const string LivenessHealthCheckEndpoint = "health/live";
    internal const string ReadinessHealthCheckEndpoint = "health/ready";
    internal const string KeepAliveEndpoint = "/";
    internal const string VersionEndpoint = "version";
    
    private const string AzureAdSwaggerClientIdKey = "AzureAd:SwaggerClientId";
    
    public static IApplicationBuilder UseCommonApi(this IApplicationBuilder app, IConfiguration configuration, Action<CommonApiApplicationBuilderOptions>? configure = null)
    {
        var commonOptions = new CommonApiApplicationBuilderOptions();
        configure?.Invoke(commonOptions);

        var clientId = configuration.GetValue<string>(AzureAdSwaggerClientIdKey);
        
        app.UseExceptionHandler();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId(clientId);
            options.OAuthUsePkce(); // ważne dla Authorization Code Flow
        });

        if (!string.IsNullOrWhiteSpace(commonOptions.CorsConfigSection))
        {
            var corsConfig = configuration.GetSection(commonOptions.CorsConfigSection).Get<CorsConfiguration>();

            if (corsConfig.AllowedOrigins.Length == 0)
            {
                throw new ConfigurationMissingException($"{commonOptions.CorsConfigSection}:{nameof(corsConfig.AllowedOrigins)}");
            }
            
            app.UseCors(builder =>
            {
                builder.AllowCredentials().WithOrigins(corsConfig.AllowedOrigins);

                _ = corsConfig.AllowedHeaders.Length > 0 ? builder.WithHeaders(corsConfig.AllowedHeaders) : builder.AllowAnyHeader();
                _ = corsConfig.AllowedMethods.Length > 0 ? builder.WithMethods(corsConfig.AllowedMethods) : builder.AllowAnyMethod();
            });
        }
        
        app.UseRouting();
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            // pinged by Azure to keep the App Service running
            endpoints.MapGet(KeepAliveEndpoint, _ => Task.CompletedTask);
            endpoints.MapGet(LivenessHealthCheckEndpoint, async context => await EndpointsService.LivenessHealthCheck(context, commonOptions.LivenessHealthCheck));
            endpoints.MapGet(ReadinessHealthCheckEndpoint, async context => await EndpointsService.ReadinessHealthCheck(context, commonOptions.ReadinessHealthCheck));
            endpoints.MapGet(VersionEndpoint, async context => await EndpointsService.Version(context));
        });

        return app;
    }
}