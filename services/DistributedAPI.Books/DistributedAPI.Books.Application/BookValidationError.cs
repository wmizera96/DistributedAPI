using System.Net;
using DistributedAPI.CommonTools;

namespace DistributedAPI.Books.Application;

public class BookValidationException : CommonBaseException
{
    public BookValidationException(HttpStatusCode httpStatusCode, string errorCode, string errorMessage, IDictionary<string, object>? parameters = null) 
        : base(httpStatusCode, errorCode, errorMessage, parameters)
    {
    }
}

public class BookValidationError
{
    public static BookValidationException ForInvalidTitle(string title)
    {
        return new BookValidationException(HttpStatusCode.BadRequest, "INVALID_TITLE", "Title {title} is invalid", new Dictionary<string, object> { { "title", title } });
    }
}