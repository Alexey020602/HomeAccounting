using Authorization.Core.CheckLoginExist;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Authorization;
//Попробовать перевести на Minimal API и попробовать добавить действия как ссылки (из REST) для действий пользователя
public static class AuthorizationEndpoints
{
    public static void MapAuthorization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/login/exist", CheckLoginExist)
            ;
    }

    private static async Task<IResult> CheckLoginExist(string login, IMediator mediator) =>
        Results.Ok(await mediator.Send(new CheckLoginExistQuery { Login = login }));
}