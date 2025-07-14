using Authorization.Contracts;
using Authorization.Core;
using Authorization.Core.Login;
using Authorization.Core.Refresh;
using Authorization.Core.Registration;
using LightResults;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Web;
using User = Authorization.Core.User;

namespace Authorization;

[AllowAnonymous]
public class AuthorizationController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await mediator.Send(new LoginQuery(loginRequest.Login, loginRequest.Password));
        if (result.IsFailure(out var error, out var authorizationResponse))
        {
            return BadRequest(error.Message);
        }

        return Ok(authorizationResponse);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        var result = await mediator.Send(new RegisterCommand(registrationRequest.Login, registrationRequest.Username, registrationRequest.Password));

        if (result.IsSuccess())
        {
            return Created();
        }

        foreach (var error in result.Errors.OfType<UserCreationError>())
        {
            ModelState.AddModelError(error.Code, error.Message);
        }
        
        return BadRequest(ModelState);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var result = await mediator.Send(new RefreshTokenQuery(refreshToken));

        if (result.IsSuccess(out var authorizationResponse, out var error))
        {
            return Ok(authorizationResponse);
        }

        if (error is not RefreshTokenError)
        {
            return BadRequest(error.Message);
        }

        return Unauthorized(
            new ProblemDetails()
            {
                Extensions = new Dictionary<string, object?>
                {
                    { "error", "invalid_grant" },
                    { "error_description", "The refresh token is invalid or expired." }
                }
            }
        );
    }
}