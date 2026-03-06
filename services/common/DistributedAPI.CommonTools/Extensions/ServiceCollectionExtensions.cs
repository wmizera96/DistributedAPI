using DistributedAPI.CommonTools.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedAPI.CommonTools.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonApi(this IServiceCollection services, IConfiguration configuration, Action<CommonApiServiceCollectionOptions>? configure = null)
    {
        var commonOptions = new CommonApiServiceCollectionOptions();
        configure?.Invoke(commonOptions);

        services.AddControllers();
        
        
        return services;
    }
}