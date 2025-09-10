using System.Security.Claims;
using Fns.Contracts;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Receipts.Contracts;
using Receipts.Core.AddReceipt;
using Receipts.Core.AddReceipt.BarCode;
using Shared.Utils.Model;
using Shared.Utils.Model.Dates;
using Shared.Web;

namespace Checks.Api;

public static class ReceiptsEndpoints
{
    public static void MapReceiptEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var receiptsGroup = endpoints
            .MapGroup("/receipts");

        receiptsGroup
            .MapGet("", GetReceipts);
        receiptsGroup
            .MapAddReceipt();

        receiptsGroup
            .MapPut(
                "/file",
                async (
                        [FromRoute] Guid budgetId,
                        ClaimsPrincipal user,
                        [FromForm] IFormFile file,
                        [FromServices] IMediator mediator) =>
                    await mediator.Send(new AddImageCheckCommand(
                        budgetId,
                        user.GetUserId(),
                        await file.OpenReadStream().ReadAsByteArrayAsync())).MapToResultAsync());
    }

    private static async Task<Ok<IReadOnlyList<CheckDto>>> GetReceipts(
        [FromRoute] Guid budgetId,
        [AsParameters] GetChecksRequest getChecks,
        // [AsParameters] GetChecksRequest checksQuery,
        // [AsParameters] DateRange range,
        [FromServices] IMediator mediator
    ) =>
        TypedResults.Ok(await mediator.Send(getChecks.ConvertToChecksQuery(budgetId)));

    public record struct AddReceiptParameters(
        [FromRoute] Guid BudgetId,
        ClaimsPrincipal User,
        [FromBody] CheckRequest CheckRequest,
        [FromServices] IMediator Mediator)
    {
        public AddCheckCommand CreateCommand => new AddCheckCommand()
        {
            ReceiptData = new(
                new ReceiptFiscalData(
                    CheckRequest.Fn,
                    CheckRequest.Fd,
                    CheckRequest.Fp,
                    CheckRequest.S,
                    CheckRequest.T
                ),
                User.GetUserId(),
                BudgetId
            ),
        };

        public ValueTask<IMaybe> PerformOperation() => Mediator.Send(CreateCommand);
    }

    private static void MapAddReceipt(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut(
            "",
            ([AsParameters] AddReceiptParameters receiptParameters) => receiptParameters
                .PerformOperation()
                .MapToResultAsync(onSuccess: TypedResults.Created)
        );
}