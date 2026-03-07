using System.Net;
using DistributedAPI.CommonTools.Services;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DistributedAPI.CommonTools.Tests;

public class BaseIntegrationTest<TStartup> where TStartup : class
{
    protected ApiCaller<TStartup> ApiCaller { get; }
    protected IEnumerable<BasePolicy> DefaultPolicies { get; set; } = new List<BasePolicy>();

    public BaseIntegrationTest()
    {
        var factory = new ApiFactory<TStartup>();
        ApiCaller = new ApiCaller<TStartup>(factory);
    }
}


public class HealthCheckTests : BaseIntegrationTest<Startup>
{
    public int Index { get; set; }
    
    public HealthCheckTests()
    {
    }

    [Fact]
    public void Test1()
    {
        ++Index;
        
        Assert.Equal(1, Index);
    }
    
    [Fact]
    public void Test2()
    {
        ++Index;
        
        Assert.Equal(1, Index);
    }

    [Fact]
    public async Task LivenessHealthCheck_WhenPass_ReturnsHealthy()
    {
        // Arrange
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).Returns(Task.CompletedTask);
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/live", Array.Empty<BasePolicy>());
        
        // Assert
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<LivenessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Live);
    }
    
    [Fact]
    public async Task LivenessHealthCheck_WhenZFails_ReturnsUnealthy()
    {
        // Arrange
        var exceptionMessage = "1the cake is a lie";
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/live", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var content = await response.GetContentAsync<LivenessHealthCheckResponse>();
        Assert.Equal(exceptionMessage, content.Status);
        Assert.False(content.Live);
    }
    
    [Fact]
    public async Task ReadinessHealthCheck_WhenPass_ReturnsHealthy()
    {
        // Arrange
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).Returns(Task.CompletedTask);
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/ready", Array.Empty<BasePolicy>());
        
        // Assert
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<ReadinessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Ready);
    }
    
    [Fact]
    public async Task ReadinessHealthCheck_WhenZFails_ReturnsUnealthy()
    {
        // Arrange
        var exceptionMessage = "2the cake is a lie";
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/ready", Array.Empty<BasePolicy>());
        
        // Assert
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var content = await response.GetContentAsync<ReadinessHealthCheckResponse>();
        Assert.Equal(exceptionMessage, content.Status);
        Assert.False(content.Ready);
    }
}
