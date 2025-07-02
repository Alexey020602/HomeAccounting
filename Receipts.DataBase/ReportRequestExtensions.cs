using System.Linq.Expressions;
using Receipts.Contracts;
using Receipts.Core;

namespace Receipts.DataBase;

internal static class ReportRequestExtensions
{
    public static Expression<Func<Entities.Check, bool>> GetPredicate(this GetChecks request) =>
        check => (!request.Range.Start.HasValue || check.PurchaseDate >= request.Range.Start.Value.ToUniversalTime())
                 && (!request.Range.End.HasValue || check.PurchaseDate <= request.Range.End.Value.ToUniversalTime())
                 && (request.Login == null || request.Login == check.Login);
}