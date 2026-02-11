using System.Net;
using System.Security.Claims;
using ClientServerShared.BarCode;
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

namespace HomeAccounting.Budgets.AddReceiptFromFile;

/// <summary>
/// Endpoint for adding receipt to a budget by uploading an image file with QR code.
/// </summary>
static class AddReceiptFromFileEndpoint
{
    /// <summary>
    /// Maps POST /budgets/{id}/receipts/file. Adds receipt from QR code image; requires edit permission.
    /// </summary>
    public static void MapAddReceiptFromFile(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{id:guid}/receipts/file",
                async (Guid id, ClaimsPrincipal user, IFormFile file, BudgetsContext budgetsContext,
                    IAuthorizationService authorizationHandler, IEventBus eventBus, IBarcodeService barcodeService,
                    CancellationToken cancellationToken) =>
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

                    // Read QR code from image
                    string qrCodeString;
                    try
                    {
                        await using var stream = file.OpenReadStream();
                        qrCodeString = await barcodeService.ReadBarcodeAsync(stream);
                    }
                    catch (BarcodeException ex)
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: $"Failed to read QR code from image: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: $"Error processing image file: {ex.Message}");
                    }

                    // Parse QR code string
                    if (!ReceiptFiscalData.TryParse(qrCodeString, out var fiscalData))
                    {
                        return Results.Problem(
                            statusCode: (int)HttpStatusCode.BadRequest,
                            detail: $"Failed to parse receipt fiscal data from QR code: '{qrCodeString}'");
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
            .WithName("AddReceiptFromFile")
            .WithTags("Budgets")
            .WithSummary("Add receipt from file")
            .WithDescription("Adds a receipt by uploading an image file with QR code. QR code is scanned, fiscal data is parsed, and receipt is processed asynchronously. Requires edit permission.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
