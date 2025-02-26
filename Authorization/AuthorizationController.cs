using DataBase.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authorization;

public class AuthorizationResponse
{
    public required string Scheme  { get; init; }
    public required string Login { get; init; }
    public required string Token { get; init; }
}

public class RegistrationRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
    public required string Username { get; init; }
}

[ApiController]
[Route("[controller]")]
public class AuthorizationController(UserManager<User> userManager, ITokenService tokenService): ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var user = await userManager.FindByIdAsync(loginRequest.Login);
        
        if (user is null) return BadRequest("User");
        
        if(!await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return BadRequest("Wrong Password");

        return Ok(
            new AuthorizationResponse
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = loginRequest.Login,
                Token = tokenService.CreateToken(user.GetClaims())
            }
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        var creationResult = await userManager.CreateAsync(
            new User
            {
                Id = registrationRequest.Login,
                UserName = registrationRequest.Username,
            },
            registrationRequest.Password
        );

        if (!creationResult.Succeeded)
        {
            foreach (var error in creationResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return Created();
    }

    [HttpPost("refresh")]
    public Task<IActionResult> Refresh()
    {
        throw new NotImplementedException();
    }
}