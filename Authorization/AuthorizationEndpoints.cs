using System.Net;
using Authorization.Contracts;
using Authorization.Core.CheckLoginExist;
using Authorization.Core.Registration;
using Authorization.Core.UpdateUser;
using Authorization.Core.UserData;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Authorization;

//Попробовать перевести на Minimal API и попробовать добавить действия как ссылки (из REST) для действий пользователя
public static class AuthorizationEndpoints
{
    public static void MapAuthorization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/users")
            .RequireAuthorization()
            .MapUserEndpoints();
    }

    private static async Task<IResult> CheckLoginExist(string login, IMediator mediator) =>
        Results.Ok(await mediator.Send(new CheckLoginExistQuery { Login = login }));
}

static class UserEndpoints
{
    public static RouteHandlerBuilder MapGetUser(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .MapGet(
                "", 
                (Guid id, IMediator mediator) => mediator.Send(new UserDataQuery(id)).MapToResultAsync())
            .Produces((int)HttpStatusCode.OK, typeof(User));

    public static RouteHandlerBuilder MapUpdateUser(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .MapPut(
                "",
                (Guid id, UpdatedUserDto user, IMediator mediator) =>
                    mediator.Send(new UpdateUserCommand(id, user)).MapToResultAsync(onSuccess: TypedResults.NoContent))
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.NotFound);

    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var userGroup = endpoints.MapGroup("/{id:guid}");
        userGroup
            .MapGetUser();

        userGroup
            .MapUpdateUser();
    }
}