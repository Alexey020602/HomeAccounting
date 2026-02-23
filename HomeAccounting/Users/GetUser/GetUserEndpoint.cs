using System.Net;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.GetUser;

/// <summary>
/// Endpoint for retrieving a user by id or username.
/// </summary>
static class GetUserEndpoint
{
    /// <summary>
    /// Maps GET /users/{id} and GET /users/{username}. Returns user by id (guid) or by username.
    /// </summary>
    public static void MapGetUser(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{id:guid}",
                async (Guid id, UserManager<User> userManager) =>
                {
                    var userId = new UserId(id);
                    if (await userManager.FindByIdAsync(userId) is not { } user)
                    {
                        return Results.NotFound($"User with id {id} does not exist");
                    }

                    return TypedResults.Ok(user.ConvertToDto());
                })
            .WithName("GetUserById")
            .WithTags("Users")
            .WithSummary("Get user by id")
            .WithDescription("Returns user by guid.")
            .Produces((int)HttpStatusCode.OK, typeof(ClientServerContracts.Users.GetUser.User))
            .ProducesProblem((int)HttpStatusCode.NotFound);

        endpoints.MapGet(
                "/{username}",
                async (string username, UserManager<User> userManager) =>
                {
                    if (await userManager.FindByNameAsync(username) is not { } user)
                    {
                        return Results.NotFound($"User with username {username} does not exist");
                    }

                    return TypedResults.Ok(user.ConvertToDto());
                })
            .WithName("GetUserByUsername")
            .WithTags("Users")
            .WithSummary("Get user by username")
            .WithDescription("Returns user by username.")
            .Produces((int)HttpStatusCode.OK, typeof(ClientServerContracts.Users.GetUser.User))
            .ProducesProblem((int)HttpStatusCode.NotFound);
    }
}