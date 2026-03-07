using DistributedAPI.CommonTools;
using Xunit;

namespace DistributedAPI.TestTools;

public class BaseIntegrationTest<TStartup> : IClassFixture<ApiFactory<TStartup>>, IAsyncLifetime where TStartup : class
{
    protected ApiCaller<TStartup> ApiCaller { get; set; }
    protected IEnumerable<BasePolicy> DefaultPolicies { get; set; } = new List<BasePolicy>();

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask InitializeAsync()
    {
        ApiCaller = new ApiCaller<TStartup>(new ApiFactory<TStartup>());
        return ValueTask.CompletedTask;
    }
}