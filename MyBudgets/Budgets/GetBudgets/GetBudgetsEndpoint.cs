using System.Net;
using System.Security.Claims;
using Budgets.Contracts.GetBudgetDetail;
using ClientServerShared.Model;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;
using BudgetUser = MyBudgets.Budgets.Data.BudgetUser;

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

    // private static ContractBudget DefaultBudget(Guid id) => new ContractBudget(id, "Ошибка получения бюджета");
}

sealed record CreateBudgetRequest(string Name, int? Limit, int BeginOfPeriod);

static class CreateBudgetEndpoint
{
    public static void MapCreateBudgets(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "", async (ClaimsPrincipal user, CreateBudgetRequest request, BudgetsContext budgetsContext, CancellationToken cancellationToken) =>
            {
                var userId = new UserId(user.GetUserId());

                if (await budgetsContext.BudgetRoles.SingleOrDefaultAsync(role => role.Name == BudgetRole.OwnerRoleName, cancellationToken: cancellationToken)
                    is not { } ownerRole)
                {
                    return Results.InternalServerError("Not found owner role for budget");
                }
                
                
                var budget = new Budget(
                    request.Name, 
                    request.BeginOfPeriod,
                    request.Limit,
                    userId,
                    DateTime.UtcNow, 
                    [new BudgetUser(userId, ownerRole.Id)]);
                
                
                budgetsContext.Budgets.Add(budget);
                
                await budgetsContext.SaveChangesAsync(cancellationToken);

                return Results.Created();
            }
        )
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}

static class GetBudgetDetailEndpoint
{
    public static void MapGetBudgetDetails(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("{id:guid}",
                (Guid id, ClaimsPrincipal user, BudgetsContext context, CancellationToken cancellationToken) =>
                {
                    
                })
            .Produces((int)HttpStatusCode.OK, typeof(BudgetDetail))
            .ProducesProblem((int)HttpStatusCode.NotFound);
    }
}