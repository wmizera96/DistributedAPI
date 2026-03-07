using Microsoft.Extensions.Hosting;

namespace DistributedAPI.CommonTools.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsLocalDev(this IHostEnvironment env) => env.IsEnvironment(CommonEnvironment.LocalDev);
    public static bool IsDEV(this IHostEnvironment env) => env.IsEnvironment(CommonEnvironment.DEV);
    public static bool IsUAT(this IHostEnvironment env) => env.IsEnvironment(CommonEnvironment.UAT);
    public static bool IsSIT(this IHostEnvironment env) => env.IsEnvironment(CommonEnvironment.SIT);
    public static bool IsPROD(this IHostEnvironment env) => env.IsEnvironment(CommonEnvironment.PROD);
}