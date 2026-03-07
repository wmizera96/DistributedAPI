using Microsoft.AspNetCore.Http;

namespace DistributedAPI.CommonTools.Extensions.Configuration;

public class CommonApiApplicationBuilderOptions
{
    internal Func<HttpContext, Task> ReadinessHealthCheck { get; private set; } = _ => Task.CompletedTask;
    internal Func<HttpContext, Task> LivenessHealthCheck { get; private set; } = _ => Task.CompletedTask;
    internal string CorsConfigSection { get; private set; } = "";

    public CommonApiApplicationBuilderOptions UseCorsPolicy(string corsConfigSection)
    {
        CorsConfigSection = corsConfigSection;

        return this;
    }
    
    public CommonApiApplicationBuilderOptions UseReadinessHealthCheck(Func<HttpContext, Task> healthCheck)
    {
        ReadinessHealthCheck = healthCheck;

        return this;
    }
    
    public CommonApiApplicationBuilderOptions UseLivenessHealthCheck(Func<HttpContext, Task> healthCheck)
    {
        LivenessHealthCheck = healthCheck;

        return this;
    }
}