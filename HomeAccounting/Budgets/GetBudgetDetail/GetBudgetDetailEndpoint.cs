using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetBudgetDetail;

/// <summary>
/// Endpoint for retrieving a single budget by id.
/// </summary>
static class GetBudgetDetailEndpoint
{
    /// <summary>
    /// Maps GET /budgets/{id}. Returns budget details if the user has read permission.
    /// </summary>
    public static void MapGetBudgetDetails(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("{id:guid}", async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Read));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to read this budget");
                }

                var budgetQuery = from budget in budgetsContext.Budgets.AsNoTracking()
                    where budget.Id == budgetId
                    select new GetBudgetsDetailResponse(
                        budget.Id.Value, 
                        budget.Name, 
                        budget.BeginOfPeriod,
                        budget.Limit.HasValue ? budget.Limit.Value.Kopecks : null);

                var response = await budgetQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken); 
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName("GetBudgetDetail")
            .WithTags("Budgets")
            .WithSummary("Get budget detail")
            .WithDescription("Returns budget details by id. Requires read permission on the budget.")
            .Produces((int)HttpStatusCode.OK, typeof(BudgetDetailDto))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }
}