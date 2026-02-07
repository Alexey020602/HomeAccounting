using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddReceiptSpending;
using ClientServerShared.Model;
using ClientServerShared.Model.Money;
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
/// Endpoint for adding receipt to a budget.
/// </summary>
static class AddReceiptSpendingEndpoint
{
    /// <summary>
    /// Maps POST /budgets/{id}/receipts. Adds receipt; requires edit permission.
    /// </summary>
    public static void MapAddReceiptSpending(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{id:guid}/receipts",
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

                    if (!await budgetsContext.Budgets.AnyAsync(b => b.Id == budgetId, cancellationToken))
                    {
                        return Results.NotFound();
                    }

                    if (request.Sum < 0)
                    {
                        return Results.BadRequest("Sum cannot be negative");
                    }

                    var fiscalData = ReceiptFiscalData.Create(request.Fn, request.Fd, request.Fp, Money.FromKopecks(request.Sum), request.PurchaseDate);
                    var existingReceipt = await budgetsContext.Receipts
                        .AnyAsync(r => r.BudgetId == budgetId &&
                                       r.FiscalData.Fn == fiscalData.Fn &&
                                       r.FiscalData.Fd == fiscalData.Fd &&
                                       r.FiscalData.Fp == fiscalData.Fp,
                            cancellationToken);

                    if (existingReceipt)
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.Conflict,
                            detail: "Receipt with these fiscal data already exists in this budget");
                    }

                    var userId = new UserId(user.GetUserId());
                    var addedDate = DateTimeOffset.UtcNow;
                    var receiptId = new ReceiptId(Guid.NewGuid());

                    var receipt = new Receipt(receiptId, budgetId, addedDate, fiscalData, userId);
                    budgetsContext.Receipts.Add(receipt);

                    var outboxEntry = ReceiptProcessingOutboxEntry.Create(receipt.Id);
                    budgetsContext.ReceiptProcessingOutbox.Add(outboxEntry);

                    await budgetsContext.SaveChangesAsync(cancellationToken);

                    await eventBus.PublishAsync(
                        new ReceiptCreated(receipt.Id),
                        cancellationToken);

                    return Results.Created();
                })
            .WithName("AddReceiptSpending")
            .WithTags("Budgets")
            .WithSummary("Add receipt")
            .WithDescription("Adds a receipt by fiscal data. Receipt is processed asynchronously. Requires edit permission.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
