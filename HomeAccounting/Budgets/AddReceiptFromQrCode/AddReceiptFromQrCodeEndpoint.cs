using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddReceiptFromQrCode;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Budgets.Events;
using HomeAccounting.Common.Infrastructure.Events.EventBus;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.AddReceiptFromQrCode;

/// <summary>
/// Endpoint for adding receipt to a budget from raw QR code string.
/// </summary>
static class AddReceiptFromQrCodeEndpoint
{
    /// <summary>
    /// Maps POST /budgets/{id}/receipts/qrcode. Adds receipt from raw QR code string; requires edit permission.
    /// </summary>
    public static void MapAddReceiptFromQrCode(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{id:guid}/receipts/qrcode",
                async (Guid id, ClaimsPrincipal user, AddReceiptFromQrCodeRequest request, BudgetsContext budgetsContext,
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

                    if (string.IsNullOrWhiteSpace(request.Raw))
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: "QR code string cannot be empty");
                    }

                    // Parse QR code string
                    if (!ReceiptFiscalData.TryParse(request.Raw, out var fiscalData))
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: $"Failed to parse receipt fiscal data from QR code: '{request.Raw}'");
                    }

                    if (fiscalData == null)
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: "Failed to parse receipt fiscal data");
                    }

                    // Create Value Objects from parsed data
                    var fn = FiscalNumber.Create(fiscalData.Fn);
                    var fd = FiscalDocument.Create(fiscalData.Fd);
                    var fp = FiscalSign.Create(fiscalData.Fp);

                    // Check for duplicate receipt
                    var existingReceipt = await budgetsContext.Receipts
                        .AnyAsync(r => r.BudgetId == budgetId &&
                                       r.Fn == fn &&
                                       r.Fd == fd &&
                                       r.Fp == fp,
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

                    var receipt = new Receipt(receiptId, budgetId, addedDate, fn, fd, fp, fiscalData.Sum, fiscalData.PurchaseDate, userId);
                    budgetsContext.Receipts.Add(receipt);

                    var outboxEntry = ReceiptProcessingOutboxEntry.Create(receipt.Id);
                    budgetsContext.ReceiptProcessingOutbox.Add(outboxEntry);

                    await budgetsContext.SaveChangesAsync(cancellationToken);

                    await eventBus.PublishAsync(
                        new ReceiptCreated(receipt.Id),
                        cancellationToken);

                    return Results.Created();
                })
            .WithName("AddReceiptFromQrCode")
            .WithTags("Budgets")
            .WithSummary("Add receipt from QR code")
            .WithDescription("Adds a receipt from raw QR code string. Fiscal data is parsed and receipt is processed asynchronously. Requires edit permission.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
