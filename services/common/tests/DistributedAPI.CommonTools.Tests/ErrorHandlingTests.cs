using System.Net;
using System.Text.Json;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.CommonTools.Test.API.Services;
using DistributedAPI.CommonTools.Test.API.Startups;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DistributedAPI.CommonTools.Tests;

public class ErrorHandlingTests : BaseIntegrationTest<ErrorStartup>
{
    [Fact]
    public async Task WhenUnhandledErrorOccurs_ReturnsExpectedErrorFormat()
    {
        // Arrange
        var errorService = new Mock<ITestErrorService>();
        errorService.Setup(s => s.ThrowException()).ThrowsAsync(new Exception());
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestErrorService>(_ => errorService.Object);
        });

        // Act
        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.Equal(CommonExceptionHandler.DefaultHttpStatusCode, response.StatusCode);        
        Assert.Equal(CommonExceptionHandler.DefaultErrorCode, content.Title);
        Assert.Equal(CommonExceptionHandler.DefaultErrorMessage, content.Detail);
    }
    
    [Fact]
    public async Task WhenCommonErrorOccurs_ReturnsExpectedErrorFormat()
    {
        // Arrange
        var httpStatusCode = HttpStatusCode.UnavailableForLegalReasons;
        var errorCode = "TEST_ERROR";
        var errorMessage = "It’s dangerous to go alone! Take this.";
        var extensions = new Dictionary<string, object>
        {
            { "min", 10 },
            { "max", 20 },
            { "values", new [] { 1, 2, 3 } },
            { "data", new { FirstName = "John", LastName = "Wick"} }
        };

        var errorService = new Mock<ITestErrorService>();
        errorService.Setup(s => s.ThrowException()).ThrowsAsync(new TestException(httpStatusCode, errorCode, errorMessage, extensions));
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestErrorService>(_ => errorService.Object);
        });

        // Act
        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.Equal(httpStatusCode, response.StatusCode);        
        Assert.Equal(errorCode, content.Title);
        Assert.Equal(errorMessage, content.Detail);

        var returnedParameters = content.Extensions["parameters"].ToString();
        var expectedParameters = JsonSerializer.Serialize(extensions, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        Assert.Equal(expectedParameters, returnedParameters);
    }
    
    [Theory]
    [InlineData(CommonEnvironment.LocalDev)]
    [InlineData(CommonEnvironment.DEV)]
    public async Task UnhandledException_WhenOnDevEnvironments_ShouldAddExceptionMessageAndStackTrace(string environmentName)
    {
        // Arrange
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environmentName);
        var exceptionMessage = "Would you kindly?";
        
        var errorService = new Mock<ITestErrorService>();
        errorService.Setup(s => s.ThrowException()).ThrowsAsync(new Exception(exceptionMessage));
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestErrorService>(_ => errorService.Object);
        });

        // Act
        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.Equal(exceptionMessage, content.Extensions["exceptionMessage"].ToString());
        Assert.NotEmpty(content.Extensions["exceptionStackTrace"].ToString());
    }
    
    [Theory]
    [InlineData(CommonEnvironment.UAT)]
    [InlineData(CommonEnvironment.SIT)]
    [InlineData(CommonEnvironment.PROD)]
    public async Task UnhandledException_WhenOnNonDevEnvironments_ShouldAddExceptionMessageAndStackTrace(string environmentName)
    {
        // Arrange
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environmentName);
        var exceptionMessage = "Would you kindly?";
        
        var errorService = new Mock<ITestErrorService>();
        errorService.Setup(s => s.ThrowException()).ThrowsAsync(new Exception(exceptionMessage));
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestErrorService>(_ => errorService.Object);
        });

        // Act
        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.False(content.Extensions.ContainsKey("exceptionMessage"));
        Assert.False(content.Extensions.ContainsKey("exceptionStackTrace"));
    }
}