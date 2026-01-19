using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgets;
using ClientServerShared.Model;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.GetBudgets;

static class GetBudgetsEndpoint
{
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
            .Produces((int) HttpStatusCode.OK, typeof(IReadOnlyCollection<ClientServerContracts.Budgets.GetBudgets.BudgetDto>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError)
            ;
    }
}