using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgets;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Budgets.GetBudgets;

/// <summary>
/// Endpoint for retrieving budgets available to the current user.
/// </summary>
static class GetBudgetsEndpoint
{
    /// <summary>
    /// Maps GET /budgets. Returns list of budgets the user has access to.
    /// </summary>
    public static void MapGetBudgets(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "",
                (ClaimsPrincipal user, BudgetsContext context) =>
                {
                    var userId = new UserId(user.GetUserId());

                    var budgets = from budget in context.Budgets.AsNoTracking()
                        where budget.BudgetUsers.Any(x => x.UserId == userId)
                        select new BudgetDto(budget.Id.Value, budget.Name);

                    return budgets.ToListAsync();
                })
            .WithName("GetBudgets")
            .WithTags("Budgets")
            .WithSummary("Get budgets")
            .WithDescription("Returns all budgets the authenticated user is a member of.")
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<ClientServerContracts.Budgets.GetBudgets.BudgetDto>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}