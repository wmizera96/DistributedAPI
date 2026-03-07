using System.Net;
using System.Reflection;
using DistributedAPI.CommonTools.Services;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.CommonTools.Test.API.Startups;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;

namespace DistributedAPI.CommonTools.Tests;

public class VersionTests : BaseIntegrationTest<VersionStartup>
{
    [Fact]
    public async Task VersionEndpoint_ExistsAndReturnsCorrectData()
    {
        // Arrange
        var assemblyName = Assembly.GetEntryAssembly().GetName();
        
        // Act
        var response = await ApiCaller.GetAsync("version", DefaultPolicies);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<VersionResponse>();
        Assert.Equal(assemblyName.Name, content.AppName);
        Assert.Equal(assemblyName.Version.ToString(), content.Version);
    }
    
    [Fact]
    public async Task VersionEndpoint_DoesNotRequireAuthorization()
    {
        // Act
        var response = await ApiCaller.GetAsync("version", Array.Empty<BasePolicy>(), false);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}