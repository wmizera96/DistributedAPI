using System.Net;
using DistributedAPI.CommonTools.Services;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;

namespace DistributedAPI.CommonTools.Tests;

public class DefaultHealthCheckTests : BaseIntegrationTest<DefaultHealthCheckStartup>
{
    [Fact]
    public async Task LivenessHealthCheck_ExistsAndPassesByDefault()
    {
        // Act
        var response = await ApiCaller.GetAsync("health/live", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<LivenessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Live);
    }
    
    [Fact]
    public async Task ReadinessHealthCheck_ExistsAndPassesByDefault()
    {
        // Act
        var response = await ApiCaller.GetAsync("health/ready", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<ReadinessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Ready);
    }
    
    [Fact]
    public async Task LivenessHealthCheck_DoesNotRequireAuthorization()
    {
        // Act
        var response = await ApiCaller.GetAsync("health/live", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task ReadinessHealthCheck_DoesNotRequireAuthorization()
    {
        // Act
        var response = await ApiCaller.GetAsync("health/live", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}