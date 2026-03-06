namespace DistributedAPI.CommonTools.Extensions.Configuration;

public class CommonApiServiceCollectionOptions
{
    internal IList<BasePolicy> Policies { get; set; } = new List<BasePolicy>();

    public CommonApiServiceCollectionOptions AddPolicy(BasePolicy policy)
    {
        Policies.Add(policy);
        return this;
    }
}