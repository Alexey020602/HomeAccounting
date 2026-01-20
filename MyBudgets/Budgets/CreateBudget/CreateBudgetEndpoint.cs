using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.CreateBudget;
using ClientServerShared.Model;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.CreateBudget;

static class CreateBudgetEndpoint
{
    public static void MapCreateBudgets(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "", async (ClaimsPrincipal user, CreateBudgetRequest request, BudgetsContext budgetsContext, CancellationToken cancellationToken) =>
                {
                    var userId = new UserId(user.GetUserId());

                    if (await budgetsContext.BudgetRoles.AsNoTracking().SingleOrDefaultAsync(role => role.Name == BudgetRole.OwnerRoleName, cancellationToken: cancellationToken)
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