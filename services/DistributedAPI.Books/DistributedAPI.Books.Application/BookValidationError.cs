using System.Net;
using DistributedAPI.CommonTools;

namespace DistributedAPI.Books.Application;

public class BookValidationError
{
    public static CommonBaseException ForInvalidTitle(string title)
    {
        return new CommonBaseException(HttpStatusCode.BadRequest, "INVALID_TITLE", "Title {title} is invalid", new Dictionary<string, object> { { "title", title } });
    }
}