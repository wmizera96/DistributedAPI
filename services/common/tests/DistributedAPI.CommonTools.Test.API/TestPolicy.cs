namespace DistributedAPI.CommonTools.Test.API;

public class TestPolicy : BasePolicy
{
    private TestPolicy(string name, string requiredRole) : base(name, requiredRole)
    {
    }
    
    public static TestPolicy Write => new TestPolicy(nameof(Write), "Test.Write");
    public static TestPolicy Read => new TestPolicy(nameof(Read), "Test.Read");
}