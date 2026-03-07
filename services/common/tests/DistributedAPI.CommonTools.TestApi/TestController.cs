using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedAPI.CommonTools.Test.API;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Authorize(nameof(TestPolicy.Read))]
    public IActionResult Get()
    {
        return Ok();
    }
    
    [HttpPost]
    [Authorize(nameof(TestPolicy.Write))]
    public IActionResult Post()
    {
        return Created();
    }
    
    [HttpGet("unprotected")]
    public IActionResult Unprotected()
    {
        return Ok();
    }
}