using Fns.Contracts;
using Microsoft.EntityFrameworkCore;
using Receipts.DataBase;

namespace Checks.DataBase;

internal static class ChecksContextExtensions
{
    public static Task<bool> CheckIsCheckExists(this ReceiptsContext context, ReceiptFiscalData checkRequest,
        CancellationToken cancellationToken = default) => context.Checks
        .AsNoTracking()
        .AnyAsync(
            c =>
                c.Fn == checkRequest.Fn &&
                c.PurchaseDate == checkRequest.T &&
                c.Fd == checkRequest.Fd &&
                c.Fp == checkRequest.Fp,
            cancellationToken
        );
}