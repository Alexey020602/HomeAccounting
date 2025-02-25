using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authorization;

[ApiController]
[Route("[controller]")]
public class AuthorizationController(UserManager<IdentityUser<Guid>> userManager): ControllerBase
{
    [HttpPost("login")]
    public Task<IActionResult> Login() => throw new NotImplementedException();
    [HttpPost("register")]
    public Task<IActionResult> Register() => throw new NotImplementedException();
    [HttpPost("refresh")]
    public Task<IActionResult> Refresh() => throw new NotImplementedException();
}