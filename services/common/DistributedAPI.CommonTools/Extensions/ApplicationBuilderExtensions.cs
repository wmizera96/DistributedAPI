using DistributedAPI.CommonTools.Extensions.Configuration;
using DistributedAPI.CommonTools.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DistributedAPI.CommonTools.Extensions;

public static class ApplicationBuilderExtensions
{
    internal const string LivenessHealthCheckEndpoint = "health/live";
    internal const string ReadinessHealthCheckEndpoint = "health/ready";
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
        
        app.UseRouting();
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet(LivenessHealthCheckEndpoint, async context => await EndpointsService.LivenessHealthCheck(context, commonOptions.LivenessHealthCheck));
            endpoints.MapGet(ReadinessHealthCheckEndpoint, async context => await EndpointsService.ReadinessHealthCheck(context, commonOptions.ReadinessHealthCheck));
            endpoints.MapGet(VersionEndpoint, async context => await EndpointsService.Version(context));
        });

        return app;
    }
}