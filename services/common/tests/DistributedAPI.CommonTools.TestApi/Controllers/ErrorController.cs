using DistributedAPI.CommonTools.Test.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedAPI.CommonTools.Test.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorController : ControllerBase
{
    private readonly ITestErrorService _testErrorService;

    public ErrorController(ITestErrorService testErrorService)
    {
        _testErrorService = testErrorService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Error()
    {
        await _testErrorService.ThrowException();
        return Ok();
    }
}