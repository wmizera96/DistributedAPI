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
    
    public static IApplicationBuilder UseCommonApi(this IApplicationBuilder app, IConfiguration configuration, Action<CommonApiApplicationBuilderOptions>? configure = null)
    {
        var commonOptions = new CommonApiApplicationBuilderOptions();
        configure?.Invoke(commonOptions);

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
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