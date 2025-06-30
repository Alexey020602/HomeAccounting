using Receipts.Contracts;
using Mediator;
using Shared.Model;

namespace Receipts.Core;

public record GetChecks(DateRange Range = new DateRange(), int? Take = null, int? Skip = null)
    : IQuery<IReadOnlyList<CheckDto>>
{
    public static explicit operator GetChecksQuery(GetChecks getChecks) => new GetChecksQuery(getChecks.Range, getChecks.Take, getChecks.Skip);
    public static explicit operator GetChecks(GetChecksQuery getChecksQuery) => new GetChecks(getChecksQuery.Range, getChecksQuery.Take, getChecksQuery.Skip);
}