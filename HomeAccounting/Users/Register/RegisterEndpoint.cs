using System.Net;
using ClientServerContracts.Users.Register;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.Register;

/// <summary>
/// Endpoint for user registration.
/// </summary>
static class RegisterEndpoint
{
    /// <summary>
    /// Maps POST /register. Creates a new user account.
    /// </summary>
    public static void MapRegister(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/register",
                async (RegistrationRequest request, UserManager<User> userManager) =>
                {
                    if (await userManager.FindByNameAsync(request.UserName) is not null)
                    {
                        return Results.ValidationProblem(
                            new Dictionary<string, string[]>
                            {
                                ["UserName"] = [$"User with username {request.UserName} already exists."]
                            });
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
            .WithName("Register")
            .WithTags("Users")
            .WithSummary("Register")
            .WithDescription("Creates a new user account with the provided credentials.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesValidationProblem()
            .AllowAnonymous();
    }
}