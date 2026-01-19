using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerShared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;

namespace MyBudgets.Budgets.GetBudgetSpendings;

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

                var budget = await budgetsContext.Budgets
                    .Include(b => b.Spendings)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                if (budget is null)
                {
                    return Results.NotFound();
                }

                var spendings = budget.Spendings
                    .Select(s => new SpendingDto(s.Id.Value, s.Description, s.Sum))
                    .ToArray();

                var response = new GetBudgetSpendingsResponse(spendings);
                return Results.Ok(response);
            })
            .Produces((int)HttpStatusCode.OK, typeof(GetBudgetSpendingsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }
}
