using System.Net;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.CheckLogin;

/// <summary>
/// Endpoint for checking if a login (username) is already taken.
/// </summary>
static class CheckLoginEndpoint
{
    /// <summary>
    /// Maps GET login/exist. Returns whether the login exists (for registration validation).
    /// </summary>
    public static void MapCheckLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "login/exist",
                async ([AsParameters] CheckLoginQuery query, UserManager<User> userManager) =>
                    Results.Ok(await userManager.FindByNameAsync(query.Login) != null))
            .WithName("CheckLogin")
            .WithTags("Users")
            .WithSummary("Check login existence")
            .WithDescription("Returns true if the login (username) is already taken. Used for registration validation.")
            .Produces((int)HttpStatusCode.OK, typeof(bool))
            .AllowAnonymous();
    }

    /// <summary>
    /// Query parameters for CheckLogin endpoint.
    /// </summary>
    public sealed record CheckLoginQuery(string Login);
}