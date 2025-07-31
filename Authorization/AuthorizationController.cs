using System.Net;
using Authorization.Contracts;
using Authorization.Core;
using Authorization.Core.CheckLoginExist;
using Authorization.Core.Login;
using Authorization.Core.Refresh;
using Authorization.Core.Registration;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Web;

namespace Authorization;



[AllowAnonymous]
public class AuthorizationController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("login/exist")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CheckLoginExist(string login) =>
        Ok(await mediator.Send(new CheckLoginExistQuery { Login = login }));
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthorizationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login(LoginRequest loginRequest) =>
        await mediator.Send(new LoginQuery(loginRequest.Login, loginRequest.Password)) switch
        {
            Some<AuthorizationResponse> authorizationResponse => Ok(authorizationResponse.Value),
            INone<AuthorizationResponse> error => BadRequest(error.Message),
            _ => throw new InvalidOperationException("Unknown operation result")
        };


    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        return await mediator.Send(new RegisterCommand(registrationRequest.Login, registrationRequest.Username, registrationRequest.Password)) switch
        {
            Some => Created(),
            INone error => BadRequest(error.ToValidationProblemDetails()),
            _ => throw new InvalidOperationException("Unknown operation result")
        };
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof (AuthorizationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof (string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails) ,(int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        return await mediator.Send(new RefreshTokenQuery(refreshToken)) switch
        {
            Some<AuthorizationResponse> authorizationResponse => Ok(authorizationResponse.Value),
            RefreshTokenError<AuthorizationResponse> => Unauthorized(
                new ProblemDetails()
                {
                    Extensions = new Dictionary<string, object?>
                    {
                        { "error", "invalid_grant" },
                        { "error_description", "The refresh token is invalid or expired." }
                    }
                }
            ),
            INone<AuthorizationResponse> error => BadRequest(error.Message),
            _ => throw new InvalidOperationException("Unknown operation result")
        };
    }
}

static class UserErrorExtensions
{
    public static ValidationProblemDetails ToValidationProblemDetails(this INone error)
    {
        var modelState = new ModelStateDictionary();

        foreach (var detail in error.Details)
        {
            modelState.AddModelError(detail.Code, detail.Description);
        }
        
        return new ValidationProblemDetails(modelState);
    }
}