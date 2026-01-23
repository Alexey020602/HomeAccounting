using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddManualSpending;
using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.AddManualSpending;

static class AddManualSpendingEndpoint
{
    public static void MapAddManualSpending(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "{id:guid}/spendings/manual",
            async (Guid id, ClaimsPrincipal user, AddManualSpendingRequest request, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Edit));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to edit this budget");
                }

                var budget = await budgetsContext.Budgets
                    .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                if (budget is null)
                {
                    return Results.NotFound();
                }

                var userId = new UserId(user.GetUserId());
                var addedDate = DateTime.UtcNow;

                budget.AddManualSpending(Money.FromKopecks(request.Sum), request.Description, request.PurchaseDate, addedDate, userId);
                
                await budgetsContext.SaveChangesAsync(cancellationToken);

                return Results.Created();
            })
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}




