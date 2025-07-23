using System.Net;
using System.Net.Mime;
using Authorization.Contracts;
using Authorization.Core.CheckLoginExist;
using Authorization.Core.Registration;
using Authorization.Core.UserData;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Authorization;

//Попробовать перевести на Minimal API и попробовать добавить действия как ссылки (из REST) для действий пользователя
public static class AuthorizationEndpoints
{
    public static void MapAuthorization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/users")
            .MapUserEndpoint()
            .RequireAuthorization();
    }

    private static async Task<IResult> CheckLoginExist(string login, IMediator mediator) =>
        Results.Ok(await mediator.Send(new CheckLoginExistQuery { Login = login }));
}

static class UserEndpoints
{
    public static RouteHandlerBuilder MapUserEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .MapGet((string)"/{id:guid}", GetUserData)
            .Produces((int)HttpStatusCode.OK, typeof(User));

    private static async Task<IResult> GetUserData(Guid id, IMediator mediator)
    {
        if ((await mediator.Send(new UserDataQuery(id))).IsFailure(out var error, out var user))
        {
            return error is UserNotFoundError userNotFoundError 
                ? Results.NotFound(new ProblemDetails()
                {
                    Detail = userNotFoundError.Message,
                })
                : Results.Problem(error.Message);
        }
        else
        {
            return Results.Ok(user);
        }
    }
}