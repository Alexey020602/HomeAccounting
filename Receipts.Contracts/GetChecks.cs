using Mediator;
using Shared.Utils.Model.Dates;

namespace Receipts.Contracts;

public record GetChecks(DateRange Range = new DateRange(), string? Login = null, int? Take = null, int? Skip = null)
    : /*PagingQuery(Take, Skip),*/ IQuery<IReadOnlyList<CheckDto>>;
