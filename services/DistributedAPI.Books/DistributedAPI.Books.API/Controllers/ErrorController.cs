using DistributedAPI.Books.Application.Model;
using Microsoft.AspNetCore.Mvc;

namespace DistributedAPI.Books.API.Controllers;

public class ErrorController : BaseController
{
    private readonly IErrorService _errorService;

    public ErrorController(IErrorService errorService)
    {
        _errorService = errorService;
    }

    [HttpGet("generic")]
    public async Task<IActionResult> GenericError()
    {
        await _errorService.GenericErrorAsync();
        return Ok();
    }
    
    [HttpGet("common/{name}")]
    public async Task<IActionResult> GenericError(string name)
    {
        await _errorService.CommonErrorAsync(name);
        return Ok();
    }
}