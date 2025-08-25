using System.Security.Claims;
using Fns.Contracts;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Receipts.Contracts;
using Receipts.Core.AddReceipt.BarCode;
using Shared.Utils.Model;
using Shared.Utils.Model.Dates;

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
            .MapPut("", AddReceipt);

        receiptsGroup
            .MapPut("/file", AddReceiptWithFile);
    }

    private static async Task<Ok<IReadOnlyList<CheckDto>>> GetReceipts(
        [FromRoute] Guid budgetId,
        [AsParameters] GetChecksRequest checksQuery,
        [AsParameters] DateRange range,
        [FromServices] IMediator mediator
        ) => 
        TypedResults.Ok(await mediator.Send(checksQuery.ConvertToChecksQuery(budgetId, range)));

    private static async Task<Ok> AddReceipt([FromRoute] Guid budgetId, ClaimsPrincipal user, [FromBody] CheckRequest checkRequest, [FromServices] IMediator mediator)
    {
        await mediator.Send(new AddCheckCommand()
        {  
            ReceiptData = new(
                new ReceiptFiscalData(
                    checkRequest.Fn,
                    checkRequest.Fd,
                    checkRequest.Fp,
                    checkRequest.S,
                    checkRequest.T
                ),
                user.GetUserId(),
                budgetId
            ),
        });
        return TypedResults.Ok();
    }

    private static async Task<Ok> AddReceiptWithFile([FromRoute] Guid budgetId, ClaimsPrincipal user, [FromForm] IFormFile file, [FromServices] IMediator mediator)
    {
        await mediator.Send(new AddImageCheckCommand()
        {
            ImageBytes = await file.OpenReadStream().ReadAsByteArrayAsync(),
            UserId = user.GetUserId(),
            BudgetId = budgetId
        });
        return TypedResults.Ok();
    }
}