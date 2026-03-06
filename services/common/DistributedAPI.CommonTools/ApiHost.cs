using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DistributedAPI.CommonTools;

public class ApiHost
{
    public static int Run<TStartup>(string[] args) where TStartup : class
    {
        try
        {
            var host = CreateHostBuilder<TStartup>(args);
            host.Build().Run();

            return 0;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Host terminated unexpectedly: {e.Message}");
            return 1;
        }
    }

    public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<TStartup>(); })
            .ConfigureAppConfiguration(SetupConfiguration);

        return hostBuilder;
    }
    
    private static void SetupConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        var keyVaultUrlString = context.Configuration["KeyVault:Url"];

        if (!string.IsNullOrWhiteSpace(keyVaultUrlString))
        {
            var keyVaultUrl = new Uri(keyVaultUrlString);
            builder.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());
        }
    }
}