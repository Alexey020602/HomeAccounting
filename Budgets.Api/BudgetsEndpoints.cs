using System.Net;
using System.Security.Claims;
using Budgets.Contracts.GetBudgets;
using Budgets.Core.GetBudgets;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Utils.Model;
using Shared.Web;

namespace Budgets.Api;

public static class BudgetsEndpoints
{
    public static void MapBudgets(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup("/budgets")
            .RequireAuthorization()
            .MapGetBudgets();
    }

    private static RouteHandlerBuilder MapGetBudgets(this IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("", GetBudgets)
        .Produces( (int) HttpStatusCode.OK, typeof(IReadOnlyCollection<Budget>))
        .ProducesProblem((int) HttpStatusCode.BadRequest);
    private static async Task<IResult> GetBudgets(GetBudgetsHttpRequest request, ClaimsPrincipal user, IMediator mediator)
    {
        return await mediator.Send(new GetBudgetsQuery(user.GetUserId())) switch
        {
            Some<IReadOnlyCollection<Budget>> someBudgets => Results.Ok(someBudgets),
            INone<IReadOnlyCollection<Budget>> error => error.MapToProblemResult(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}