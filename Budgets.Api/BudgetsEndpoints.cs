using System.Net;
using System.Security.Claims;
using Budgets.Contracts.CreateBudget;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Contracts.GetBudgets;
using Budgets.Contracts.UserInBudgetPermissions;
using Budgets.Core.CreateBudget;
using Budgets.Core.EditBudget;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.GetBudgets;
using Budgets.Core.UserInBudgetPermissions;
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
        endpoints.MapBudgetsGroup().MapBudgetDetailGroup();

    private static RouteGroupBuilder MapBudgetDetailGroup(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGroup("{budgetId:guid}");
    public static void MapBudgets(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapBudgetsGroup();
        group.MapGetBudgets();
        group.MapCreateBudget();

        var budgetGroup = group.MapBudgetDetailGroup();
        budgetGroup
            .MapGetBudgetDetail();
        budgetGroup
            .MapUpdateBudget();
    }

    private static RouteGroupBuilder MapBudgetsGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("/budgets")
            // .WithGroupName("Budgets")
            .RequireAuthorization();
    }
    private static RouteHandlerBuilder MapCreateBudget(this IEndpointRouteBuilder endpoints) => endpoints
        .MapPost("", EndpointDelegates.CreateBudget)
        // .WithName("Create budget")
        .Produces((int)HttpStatusCode.Created)
        .ProducesValidationProblem()
        .Produces((int)HttpStatusCode.InternalServerError);

    private static RouteHandlerBuilder MapGetBudgets(this IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("", EndpointDelegates.GetBudgets)
        // .WithName("Get budgets")
        .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<Budget>))
        .ProducesProblem((int)HttpStatusCode.BadRequest);

    private static RouteHandlerBuilder MapGetBudgetDetail(this IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("", EndpointDelegates.GetBudgetDetail)
        // .WithName("Get budget detail")
        .Produces((int)HttpStatusCode.OK, typeof(BudgetFullDetail))
        .ProducesProblem((int)HttpStatusCode.BadRequest)
        .ProducesProblem((int)HttpStatusCode.InternalServerError);
    
    private static RouteHandlerBuilder MapUpdateBudget(this IEndpointRouteBuilder endpoints) => endpoints
    .MapPut("", EndpointDelegates.UpdateBudget)
    .Produces((int)HttpStatusCode.NoContent)
    .Produces((int) HttpStatusCode.Forbidden)
    .ProducesValidationProblem();
}

internal static class EndpointDelegates
{
    public static Task<Results<NoContent, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> UpdateBudget(
        Guid budgetId,
        ClaimsPrincipal user, 
        BudgetData budget,
        IMediator mediator
        ) => mediator.Send(new UpdateBudgetCommand(budgetId, user, budget)).AsTask().MapToResultAsync(TypedResults.NoContent);
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

    public static async Task<Results<Ok<BudgetFullDetail>, ForbidHttpResult, NotFound, InternalServerError<BudgetFullDetail>>>
        GetBudgetDetail(
            Guid budgetId,
            ClaimsPrincipal user,
            IMediator mediator
        )
    {
        return await mediator.Send(new GetBudgetQuery(budgetId, user)) switch
        {
            Some<BudgetFullDetail> detail => TypedResults.Ok(detail.Value),
            Core.UserInBudgetPermissions.UserHasNoPermission<BudgetFullDetail> => TypedResults.Forbid(),
            BudgetNotFoundError<BudgetFullDetail> => TypedResults.NotFound(),
            // INone<BudgetDetail> error => error.MapToProblemResult(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }

    public static async Task<Results<Ok<UserInBudgetPermissions>, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult> >> GetUserInBudgetPermissions(
        Guid budgetId,
        ClaimsPrincipal user,
        IMediator mediator
        ) =>
        (await mediator.Send(new UserInBudgetPermissionsQuery(user.GetUserId(), budgetId))).MapToResult();
    
    
}