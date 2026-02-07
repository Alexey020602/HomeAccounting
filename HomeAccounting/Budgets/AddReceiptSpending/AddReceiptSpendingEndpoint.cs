using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddReceiptSpending;
using ClientServerShared.Model;
using ClientServerShared.Model.Money;
// using ClientServerShared.Model.Money;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Budgets.Events;
using HomeAccounting.Common.Infrastructure.Events.EventBus;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.AddReceiptSpending;

/// <summary>
/// Endpoint for adding receipt-based spending to a budget.
/// </summary>
static class AddReceiptSpendingEndpoint
{
    /// <summary>
    /// Maps POST /budgets/{id}/spendings/receipt. Adds spending from receipt; requires edit permission.
    /// </summary>
    public static void MapAddReceiptSpending(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{id:guid}/spendings/receipt",
                async (Guid id, ClaimsPrincipal user, AddReceiptSpendingRequest request, BudgetsContext budgetsContext,
                    IAuthorizationService authorizationHandler, IEventBus eventBus, CancellationToken cancellationToken) =>
                {
                    var budgetId = new BudgetId(id);
                    var result = await authorizationHandler.AuthorizeAsync(user, budgetId,
                        new BudgetRequirements(BudgetPermissions.Edit));
                    if (!result.Succeeded)
                    {
                        return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden,
                            detail: "User does not have permission to edit this budget");
                    }

                    var budget = await budgetsContext.Budgets
                        .Include(b => b.Spendings)
                        .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                    if (budget is null)
                    {
                        return Results.NotFound();
                    }

                    if (request.Sum < 0)
                    {
                        return Results.BadRequest("Sum cannot be negative");
                    }

                    // Проверяем, что чека с такими фискальными данными еще нет
                    var fiscalData = ReceiptFiscalData.Create(request.Fn, request.Fd, request.Fp, Money.FromKopecks(request.Sum), request.PurchaseDate);
                    var existingReceipt = budget.Spendings
                        .OfType<ReceiptSpending>()
                        .FirstOrDefault(rs => rs.FiscalData.Fn == fiscalData.Fn &&
                                               rs.FiscalData.Fd == fiscalData.Fd &&
                                               rs.FiscalData.Fp == fiscalData.Fp);

                    if (existingReceipt is not null)
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.Conflict,
                            detail: "Receipt with these fiscal data already exists in this budget");
                    }

                    var userId = new UserId(user.GetUserId());
                    var addedDate = DateTimeOffset.UtcNow;

                    var receiptSpending = budget.AddReceiptSpending(addedDate, fiscalData, userId);

                    await budgetsContext.SaveChangesAsync(cancellationToken);

                    // Публикуем событие для асинхронной обработки в фоне
                    await eventBus.PublishAsync(
                        new ReceiptCreated(receiptSpending.Id),
                        cancellationToken);

                    return Results.Created();
                })
            .WithName("AddReceiptSpending")
            .WithTags("Budgets")
            .WithSummary("Add receipt spending")
            .WithDescription("Adds spending from a receipt by fiscal data. Receipt is processed asynchronously. Requires edit permission.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
