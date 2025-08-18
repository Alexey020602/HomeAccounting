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
    public static RouteGroupBuilder MapBudgetGroup(this IEndpointRouteBuilder endpoints) => endpoints.MapBudgetsGroup().MapGroup("{budgetId:guid}");
    public static RouteHandlerBuilder MapBudgets(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapBudgetsGroup()
            .MapGetBudgets();
    }
    private static RouteGroupBuilder MapBudgetsGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("/budgets")
            .RequireAuthorization();
    }
    private static RouteHandlerBuilder MapGetBudgets(this IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("", GetBudgets)
        .Produces( (int) HttpStatusCode.OK, typeof(IReadOnlyCollection<Budget>))
        .ProducesProblem((int) HttpStatusCode.BadRequest);
    private static async Task<IResult> GetBudgets([AsParameters] GetBudgetsHttpRequest request, ClaimsPrincipal user, IMediator mediator)
    {
        return await mediator.Send(new GetBudgetsQuery(user.GetUserId())) switch
        {
            // Some<List<Budget>> otherSomeBudgets => TypedResults.Ok(otherSomeBudgets.Value),
            Some<IReadOnlyCollection<Budget>> someBudgets => Results.Ok(someBudgets.Value),
            INone<IReadOnlyCollection<Budget>> error => error.MapToProblemResult(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}