using DistributedAPI.Books.Application.Model;
using DistributedAPI.TestTools;
using DistributedAPI.TestTools.Extensions;

namespace DistributedAPI.Books.API.IntegrationTests;

public class BooksControllerTests : BaseIntegrationTest<Startup>
{
    public BooksControllerTests(ApiFactory<Startup> apiFactory) : base(apiFactory)
    {
        DefaultPolicies = new [] { BooksPolicy.Read,  BooksPolicy.Write };
    }

    [Fact]
    public async Task GetBooks_WhenCalled_ReturnsOkResult()
    {
        // Act
        var response = await ApiCaller.GetAsync("api/books", DefaultPolicies);
        
        // Assert
        var content = await response.GetContentAsync<IEnumerable<Book>>();
        
        Assert.Equal(content.Count(), 2);
    }
}
