using Authorization.Contracts;
using Authorization.Extensions;
using Authorization.Models;
using Checks.DataBase;
using Checks.DataBase.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Web;
using User = Checks.DataBase.Entities.User;

namespace Authorization;

[AllowAnonymous]
public class AuthorizationController( /*IUserService userService, */UserManager<User> userManager,
    ITokenService tokenService) : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var user = await userManager.FindByIdAsync(loginRequest.Login);
        // var user = await userService.GetUserByLogin(loginRequest.Login);

        if (user is null) return BadRequest("User not found");

        if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return BadRequest("Wrong Password");

        var userRefreshToken = await CreateRefreshToken(user);
        
        return Ok(
            new AuthorizationResponse
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = loginRequest.Login,
                AccessToken = CreateAccessToken(user),
                RefreshToken = userRefreshToken.Token,
            }
        );
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        var existingUser = await userManager.FindByIdAsync(registrationRequest.Login);
        if (existingUser is not null) return BadRequest("User already exists");

        var creationResult = await userManager.CreateAsync(
            new User
            {
                Id = registrationRequest.Login,
                UserName = registrationRequest.Username,
            },
            registrationRequest.Password
        );

        if (creationResult.Succeeded)
        {
            return Created();
        }

        foreach (var error in creationResult.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(user =>
            user.RefreshTokenToken != null && user.RefreshTokenToken.Token == refreshToken);
        if (user is null)
        {
            return BadRequest("User Not Found");
        }

        if (user.RefreshTokenToken is null || user.RefreshTokenToken.Expires < DateTime.UtcNow)
        {
            return Unauthorized(
                new ProblemDetails()
                {
                    Extensions = new Dictionary<string, object?>  
                    {
                        {"error", "invalid_grant"},
                        {"error_description", "The refresh token is invalid or expired."}
                    }
                }
                );
        }

        var userRefreshToken = await CreateRefreshToken(user);

        return Ok(
            new AuthorizationResponse()
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = user.Id,
                AccessToken = CreateAccessToken(user),
                RefreshToken = userRefreshToken.Token,
            }
        );
    }
    private string CreateAccessToken(User user)
    {
        return tokenService.CreateToken(user.GetClaims());
    }
    private async Task<RefreshToken> CreateRefreshToken(User user)
    {
        var refreshToken = tokenService.CreateRefreshToken();
        user.RefreshTokenToken = refreshToken;

        await userManager.UpdateAsync(user);
        return refreshToken;
    }
}

