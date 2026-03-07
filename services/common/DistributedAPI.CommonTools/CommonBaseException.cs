using System.Net;

namespace DistributedAPI.CommonTools;

public class CommonBaseException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public IDictionary<string, object> Parameters { get; set; }

    public CommonBaseException(HttpStatusCode httpStatusCode, string errorCode, string errorMessage, IDictionary<string, object>? parameters = null)
    {
        HttpStatusCode = httpStatusCode;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Parameters = parameters ?? new Dictionary<string, object>();
    }
}