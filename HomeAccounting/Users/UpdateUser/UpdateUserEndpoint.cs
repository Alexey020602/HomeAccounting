using System.Net;
using ClientServerContracts.User.UpdateUser;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.UpdateUser;

static class UpdateUserEndpoint
{
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
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.NotFound)
            ;
    }
}