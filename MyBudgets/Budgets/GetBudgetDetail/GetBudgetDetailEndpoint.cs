using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;

namespace MyBudgets.Budgets.GetBudgetDetail;

internal static class GetBudgetDetailEndpoint
{
    public static void MapGetBudgetDetails(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("{id:guid}", async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, CancellationToken cancellationToken) =>
            {
                var budgetId = new  BudgetId(id);
                var budgetQuery = from budget in budgetsContext.Budgets.AsNoTracking()
                    where budget.Id == budgetId
                    select new GetBudgetsDetailResponse(
                        budget.Id.Value, 
                        budget.Name, 
                        budget.BeginOfPeriod, 
                        budget.Limit);

                var response = await budgetQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken); 
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
            .Produces((int)HttpStatusCode.OK, typeof(BudgetDetailDto))
            .ProducesProblem((int)HttpStatusCode.NotFound);
    }
}