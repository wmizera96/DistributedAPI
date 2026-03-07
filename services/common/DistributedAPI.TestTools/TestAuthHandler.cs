using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DistributedAPI.TestTools;


public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly TestUserContext _userContext;

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, TestUserContext userContext) : base(options, logger, encoder)
    {
        _userContext = userContext;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!_userContext.IsAuthenticated)
        {
            return Task.FromResult(AuthenticateResult.Fail("user not authenticated"));
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
        };

        foreach (var policy in _userContext.Policies)
        {
            claims.Add(new Claim(ClaimTypes.Role, policy.RequiredRole));
        }
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}