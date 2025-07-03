using Shared.Model.Dates;

namespace Reports.Contracts;

public record ReportRequest(DateRange Range);