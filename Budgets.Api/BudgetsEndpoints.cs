using System.Net;
using System.Security.Claims;
using Budgets.Contracts.CreateBudget;
using Budgets.Contracts.GetBudgets;
using Budgets.Core.CreateBudget;
using Budgets.Core.GetBudgets;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Utils.Model;
using Shared.Web;

namespace Budgets.Api;

public static class BudgetsEndpoints
{
    public static RouteGroupBuilder MapBudgetGroup(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapBudgetsGroup().MapGroup("{budgetId:guid}");

    public static void MapBudgets(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapBudgetsGroup();
        group.MapGetBudgets();
        group.MapCreateBudget();
    }

    private static RouteGroupBuilder MapBudgetsGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("/budgets")
            .RequireAuthorization();
    }
    private static RouteHandlerBuilder MapCreateBudget(this IEndpointRouteBuilder endpoints) => endpoints
        .MapPost("", EndpointDelegates.CreateBudget)
        .Produces((int)HttpStatusCode.Created)
        .ProducesValidationProblem()
        .Produces((int)HttpStatusCode.InternalServerError);

    private static RouteHandlerBuilder MapGetBudgets(this IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("", EndpointDelegates.GetBudgets)
        .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<Budget>))
        .ProducesProblem((int)HttpStatusCode.BadRequest);
}

internal static class EndpointDelegates
{
    public static async Task<Results<Created, ValidationProblem, InternalServerError>> CreateBudget(
        ClaimsPrincipal user,
        CreateBudgetHttpRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
                new CreateBudgetCommand(user.GetUserId(), request.Name, request.BeginOfPeriod, request.Limit,
                    DateTime.UtcNow), cancellationToken) switch
            {
                Some _ => TypedResults.Created(),
                INone error => error.MapToValidationProblemResult(),
                _ => TypedResults.InternalServerError()
            };
    }

    public static async Task<Results<Ok<IReadOnlyCollection<Budget>>, ProblemHttpResult>> GetBudgets(
        [AsParameters] GetBudgetsHttpRequest request, ClaimsPrincipal user, IMediator mediator)
    {
        return await mediator.Send(new GetBudgetsQuery(user.GetUserId())) switch
        {
            // Some<List<Budget>> otherSomeBudgets => TypedResults.Ok(otherSomeBudgets.Value),
            Some<IReadOnlyCollection<Budget>> someBudgets => TypedResults.Ok(someBudgets.Value),
            INone<IReadOnlyCollection<Budget>> error => error.MapToProblemResult(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}