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
        var exceptionMessage = "Would you kindly?";
        
        var errorService = new Mock<ITestErrorService>();
        errorService.Setup(s => s.ThrowException()).ThrowsAsync(new Exception(exceptionMessage));
        ApiCaller.ConfigureCustomServices(services =>
        {
            services.AddScoped<ITestErrorService>(_ => errorService.Object);
        });

        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.Equal(CommonExceptionHandler.DefaultHttpStatusCode, response.StatusCode);        
        Assert.Equal(CommonExceptionHandler.DefaultErrorCode, content.Title);
        Assert.Equal(CommonExceptionHandler.DefaultErrorMessage, content.Detail);

        Assert.Equal(exceptionMessage, content.Extensions["exceptionMessage"].ToString());
        Assert.NotEmpty(content.Extensions["exceptionStackTrace"].ToString());
    }
    
    [Fact]
    public async Task WhenCommonErrorOccurs_ReturnsExpectedErrorFormat()
    {
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

        var response = await ApiCaller.GetAsync("api/error", DefaultPolicies);
        
        var content = await response.GetContentAsync<ProblemDetails>();
        
        Assert.Equal(httpStatusCode, response.StatusCode);        
        Assert.Equal(errorCode, content.Title);
        Assert.Equal(errorMessage, content.Detail);

        var parameters = (JsonElement)content.Extensions["parameters"];
        var minValue = parameters.GetProperty("min").GetInt32();
        var maxValue = parameters.GetProperty("max").GetInt32();
        
        Assert.Equal(10, minValue);
        Assert.Equal(20, maxValue);

    }
}