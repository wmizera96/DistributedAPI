using System.Net;
using DistributedAPI.CommonTools.Services;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.CommonTools.Test.API.Startups;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DistributedAPI.CommonTools.Tests;


public class CustomHealthCheckTests : BaseIntegrationTest<CustomHealthCheckStartup>
{
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
        var response = await ApiCaller.GetAsync("health/live", DefaultPolicies);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<LivenessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Live);
    }
    
    [Fact]
    public async Task LivenessHealthCheck_WhenZFails_ReturnsUnealthy()
    {
        // Arrange
        var exceptionMessage = "The cake is a lie.";
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/live", DefaultPolicies);
        
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
        var response = await ApiCaller.GetAsync("health/ready", DefaultPolicies);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.GetContentAsync<ReadinessHealthCheckResponse>();
        Assert.Equal("ok", content.Status);
        Assert.True(content.Ready);
    }
    
    [Fact]
    public async Task ReadinessHealthCheck_WhenZFails_ReturnsUnealthy()
    {
        // Arrange
        var exceptionMessage = "There is no spoon.";
        var serviceMock = new Mock<ITestHealthCheckService>();
        serviceMock.Setup(m => m.RunAsync()).ThrowsAsync(new Exception(exceptionMessage));
        
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestHealthCheckService>(_ => serviceMock.Object);
        });
        
        // Act
        var response = await ApiCaller.GetAsync("health/ready", DefaultPolicies);
        
        // Assert
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var content = await response.GetContentAsync<ReadinessHealthCheckResponse>();
        Assert.Equal(exceptionMessage, content.Status);
        Assert.False(content.Ready);
    }
}
