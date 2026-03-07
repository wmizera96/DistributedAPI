using DistributedAPI.CommonTools;
using Xunit;

namespace DistributedAPI.TestTools;

public class BaseIntegrationTest<TStartup> : IClassFixture<ApiFactory<TStartup>> where TStartup : class
{
    protected ApiCaller<TStartup> ApiCaller { get; }
    protected IEnumerable<BasePolicy> DefaultPolicies { get; set; } = new List<BasePolicy>();

    public BaseIntegrationTest(ApiFactory<TStartup> apiFactory)
    {
        ApiCaller = new ApiCaller<TStartup>(apiFactory);
    }
}