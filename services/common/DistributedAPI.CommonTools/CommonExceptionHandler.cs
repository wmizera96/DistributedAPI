using System.Net;
using DistributedAPI.CommonTools.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace DistributedAPI.CommonTools;

public class CommonExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _env;
    public const HttpStatusCode DefaultHttpStatusCode = HttpStatusCode.InternalServerError;
    public const string DefaultErrorCode = "UNHANDLED_EXCEPTION";
    public const string DefaultErrorMessage = "Unhandled exception was thrown by the application. Please try again and contact support if the problem persists.";

    public CommonExceptionHandler(IHostEnvironment env)
    {
        _env = env;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = exception is CommonBaseException cbe
            ? CreateCommonProblemDetails(cbe)
            : CreateGenericProblemDetails();
        
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        
        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        if (_env.IsLocalDev() || _env.IsDEV())
        {
            problemDetails.Extensions["exceptionMessage"] = exception.Message;
            problemDetails.Extensions["exceptionStackTrace"] = exception.StackTrace;
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails CreateCommonProblemDetails(CommonBaseException exception)
    {
        var details = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)exception.HttpStatusCode}",
            Status = (int)exception.HttpStatusCode,
            Title = exception.ErrorCode,
            Detail = exception.ErrorMessage,
        };
        
        details.Extensions.Add("parameters", exception.Parameters);
        
        return details;
    }

    private ProblemDetails CreateGenericProblemDetails()
    {
        return new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)DefaultHttpStatusCode}",
            Status = (int)DefaultHttpStatusCode,
            Title = DefaultErrorCode,
            Detail = DefaultErrorMessage,
        };
    }
}