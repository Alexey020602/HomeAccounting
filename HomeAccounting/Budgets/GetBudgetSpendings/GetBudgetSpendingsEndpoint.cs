using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetBudgetSpendings;

internal static class GetBudgetSpendingsEndpoint
{
    public static void MapGetBudgetSpendings(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/spendings",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Read));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to read this budget");
                }

                if (!await budgetsContext.Budgets.AnyAsync(budget => budget.Id == budgetId, cancellationToken: cancellationToken))
                {
                    return Results.NotFound();
                }

                var spendingsQuery = from budget in budgetsContext.Budgets.AsNoTracking()
                    where budget.Id == budgetId
                    from spending in budget.Spendings
                    
                    select new SpendingDto(spending.Id.Value, spending.Description, spending.Sum.Kopecks);
                    
                
                var spendings = await spendingsQuery
                    .ToArrayAsync(cancellationToken);

                var response = new GetBudgetSpendingsResponse(spendings);
                return Results.Ok(response);
            })
            .Produces((int)HttpStatusCode.OK, typeof(GetBudgetSpendingsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }
}
