using System.Net;
using System.Security.Claims;
using ClientServerShared.Model;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.GetBudgets;
using ContractBudget = ClientServerContracts.Budgets.GetBudgets.Budget;

static class GetBudgetsEndpoint
{
    public static void MapGetBudgets(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "",
             (ClaimsPrincipal user, BudgetsContext context) =>
            {
                var userId = new UserId(user.GetUserId());

                var budgets = from budget in context.Budgets
                    where budget.BudgetUsers.Any(x => x.UserId == userId)
                    select new ContractBudget(budget.Id.Value, budget.Name);

                return budgets.ToListAsync();
                // return context.BudgetUsers
                //     .Where(u => u.UserId == userId)
                //     .Select(u =>  new ContractBudget(u.Budget.Id.Value, u.Budget.Name))
                //     .ToListAsync();
            })
            .Produces((int) HttpStatusCode.OK, typeof(IReadOnlyCollection<ClientServerContracts.Budgets.GetBudgets.Budget>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError)
            ;
    }

    private static ContractBudget DefaultBudget(Guid id) => new ContractBudget(id, "Ошибка получения бюджета");
}