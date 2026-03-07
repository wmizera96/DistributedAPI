using DistributedAPI.CommonTools;

namespace DistributedAPI.TestTools;

internal record TestUserContext(IEnumerable<BasePolicy> Policies, bool IsAuthenticated);
