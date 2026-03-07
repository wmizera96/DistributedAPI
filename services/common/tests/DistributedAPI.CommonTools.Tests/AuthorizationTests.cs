using System.Net;
using DistributedAPI.CommonTools.Test.API;
using DistributedAPI.CommonTools.Test.API.Startups;
using DistributedAPI.TestTools;

namespace DistributedAPI.CommonTools.Tests;

public class AuthorizationTests : BaseIntegrationTest<AuthorizationStartup>
{
    [Fact]
    public async Task Get_WhenMissingPermissions_ReturnsForbidden()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/test", new[] { TestPolicy.Write });
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_WhenHasPermissions_ReturnsOk()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/test", new[] { TestPolicy.Read });
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_WhenUnauthenticated_ReturnsUnauthorized()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/test", new[] { TestPolicy.Read }, false);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_WhenHasPermissions_ReturnsOk()
    {
        // Act
        var response = await ApiCaller.PostAsync("api/test", new {}, new[] { TestPolicy.Write });
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_WhenUnauthenticated_ReturnsUnauthorized()
    {
        // Act
        var response = await ApiCaller.PostAsync("api/test", new{}, new[] { TestPolicy.Read }, false);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task GetUnprotected_WhenUnauthenticated_ReturnsOK()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/test/unprotected", new[] { TestPolicy.Read }, false);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}