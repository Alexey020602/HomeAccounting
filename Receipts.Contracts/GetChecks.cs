using Mediator;
using Shared.Utils.Model.Dates;

namespace Receipts.Contracts;

public record GetChecks(
    Guid BudgetId,
    DateRange Range = new DateRange(),
    Guid? Login = null,
    int? Take = null,
    int? Skip = null)
    : /*PagingQuery(Take, Skip),*/ IQuery<IReadOnlyList<CheckDto>>;