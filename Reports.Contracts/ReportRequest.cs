using Shared.Utils.Model.Dates;

namespace Reports.Contracts;

public record ReportRequest(Guid BudgetId, DateRange Range);