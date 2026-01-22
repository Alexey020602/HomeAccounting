using ClientServerShared.Users;
using Microsoft.AspNetCore.Identity;
using MyBudgets.Users.Data;

namespace MyBudgets.Users.GetUser;

static class GetUserEndpoint
{
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
            }
        );
        endpoints.MapGet(
            "/{username}",
            async (string username, UserManager<User> userManager) =>
            {
                if (await userManager.FindByNameAsync(username) is not { } user)
                {
                    return Results.NotFound($"User with username {username} does not exist");
                }
                
                return TypedResults.Ok(user.ConvertToDto());
            }
        );
    }

}