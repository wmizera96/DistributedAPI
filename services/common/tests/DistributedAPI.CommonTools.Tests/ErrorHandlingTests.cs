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
        Assert.Equal(exceptionMessage, content.Extensions["exceptionMessage"]);
        
    }
}