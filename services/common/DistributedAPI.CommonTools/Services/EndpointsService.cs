using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace DistributedAPI.CommonTools.Services;

public class EndpointsService
{
    public static async Task LivenessHealthCheck(HttpContext context, Func<HttpContext, Task> healthCheck)
    {
        try
        {
            await healthCheck(context);
            var response = new LivenessHealthCheckResponse(true, "ok");
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception e)
        {
            var response = new  LivenessHealthCheckResponse(false, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
    
    public static async Task ReadinessHealthCheck(HttpContext context, Func<HttpContext, Task> healthCheck)
    {
        try
        {
            await healthCheck(context);
            var response = new ReadinessHealthCheckResponse(true, "ok");
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception e)
        {
            var response = new  ReadinessHealthCheckResponse(false, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
    
    public static async Task Version(HttpContext context)
    {
        var assemblyName = Assembly.GetEntryAssembly().GetName();
        var response = new VersionResponse(assemblyName.Name, assemblyName.Version.ToString());
        
        await context.Response.WriteAsJsonAsync(response);
    }
}