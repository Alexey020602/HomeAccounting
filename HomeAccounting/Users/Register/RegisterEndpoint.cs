using System.Net;
using ClientServerContracts.User.Register;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.Register;

static class RegisterEndpoint
{
    public static void MapRegister(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/register",
                async (RegistrationRequest request, UserManager<User> userManager) =>
                {
                    if (await userManager.FindByNameAsync(request.UserName) is not null)
                    {
                        return Results.BadRequest($"User with username {request.UserName} already exists.");
                    }

                    var user = new User(request.UserName, request.FullName);
                    var creationResult = await userManager.CreateAsync(
                        user,
                        request.Password
                    );
                    
                    if (creationResult.Succeeded)
                    {
                        return Results.Created();
                    }
                    
                    return Results.ValidationProblem(
                        creationResult.Errors.Select(e => new KeyValuePair<string, string[]>(e.Code, [e.Description]))
                    );
                }
            )
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.BadRequest)
            .AllowAnonymous();
    }
}