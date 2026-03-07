using System.Net;

namespace DistributedAPI.CommonTools.Test.API;

public class TestException : CommonBaseException
{
    public TestException(HttpStatusCode httpStatusCode, string errorCode, string errorMessage, IDictionary<string, object>? parameters = null) 
        : base(httpStatusCode, errorCode, errorMessage, parameters)
    {
    }
}