using Shared.Model;

namespace Receipts.Contracts;

public record GetChecksQuery(DateRange Range = new DateRange(), int? Take = null, int? Skip = null): PagingQuery(Take, Skip);