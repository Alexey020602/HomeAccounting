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
            .MapGet("", GetUserData)
            .Produces((int)HttpStatusCode.OK, typeof(User));

    public static RouteHandlerBuilder MapUpdateUser(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .MapPut("", UpdateUser)
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

    private static async Task<IResult> GetUserData(Guid id, IMediator mediator) =>
        await mediator.Send(new UserDataQuery(id)) switch
        {
            Some<User> user => Results.Ok(user.Value),
            UserNotFoundError<User> userNotFoundError => Results.NotFound(new ProblemDetails()
            {
                Detail = userNotFoundError.Message,
            }),
            INone<User> error => Results.Problem(error.Message),
            _ => Results.InternalServerError()
        };

    private static Task<Results<NoContent, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> UpdateUser(
        Guid id,
        UpdatedUserDto user,
        IMediator mediator
    ) => mediator.Send(new UpdateUserCommand(id, user)).MapToResultAsync(onSuccess: TypedResults.NoContent);
}