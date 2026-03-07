using DistributedAPI.CommonTools;

namespace DistributedAPI.TestTools;

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