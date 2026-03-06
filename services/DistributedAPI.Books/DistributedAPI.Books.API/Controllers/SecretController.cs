using Microsoft.AspNetCore.Mvc;

namespace DistributedAPI.Books.API.Controllers;

public class SecretController : BaseController
{
    private readonly IConfiguration _configuration;

    public SecretController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetSecret()
    {
        var secret = _configuration["MySecret"];
        return Ok(secret);
    }
}