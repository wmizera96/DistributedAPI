namespace DistributedAPI.CommonTools;

public class BasePolicy
{
    public string Name { get; set; }
    public string RequiredRole { get; set; }

    protected BasePolicy(string name, string requiredRole)
    {
        Name = name;
        RequiredRole = requiredRole;
    }
}
