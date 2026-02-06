using System.Net;
using ClientServerContracts.Users.UpdateUser;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.UpdateUser;

/// <summary>
/// Endpoint for updating user profile.
/// </summary>
static class UpdateUserEndpoint
{
    /// <summary>
    /// Maps PUT /users/{id}. Updates user full name and username.
    /// </summary>
    public static void MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "{id:guid}",
            async (Guid id, UpdatedUserDto updatedUser, UserManager<User> userManager) =>
            {
                var userId = new UserId(id);
                if (await userManager.FindByIdAsync(userId) is not { } existingUser)
                {
                    return Results.NotFound("User with id {id} not found");
                }
                
                existingUser.UpdateFullName(updatedUser.FullName);
                existingUser.UpdateUserName(updatedUser.UserName);

                if (await userManager.UpdateAsync(existingUser) is not { Succeeded: false } result)
                {
                    return Results.NoContent();
                }

                return Results.ValidationProblem(
                    result.Errors.Select(e => new KeyValuePair<string, string[]>(e.Code, [e.Description]))
                );
            })
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithSummary("Update user")
            .WithDescription("Updates user full name and username by id.")
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.NotFound);
    }
}