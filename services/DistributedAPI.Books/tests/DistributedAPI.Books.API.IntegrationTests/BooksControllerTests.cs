using System.Net;
using DistributedAPI.Books.Application.Model;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;

namespace DistributedAPI.Books.API.IntegrationTests;

public class BooksControllerTests : BaseIntegrationTest<Startup>
{
    public BooksControllerTests()
    {
        DefaultPolicies = new [] { BooksPolicy.Read,  BooksPolicy.Write };
    }

    [Fact]
    public async Task GetBooks_WhenCalledWithInsufficientPermissions_Returns403()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/books", new [] { BooksPolicy.Write });
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task GetBooks_WhenCalled_ReturnsOkResult()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/books", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<IEnumerable<Book>>();
        
        Assert.Equal(2, content.Count());
    }
    
    [Fact]
    public async Task PostBook_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var data = new Book("someTitle", "someDescription", DateTime.Now);
        
        // Act
        var response = await ApiCaller.PostAsync("api/books", data, DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<Book>();
        
        Assert.Equal(data.Title, content.Title);
        Assert.Equal(data.Author, content.Author);
        Assert.Equal(data.PublicationDate, content.PublicationDate);
    }
}
