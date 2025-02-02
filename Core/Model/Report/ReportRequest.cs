namespace Core.Model.Report;

public record ReportRequest(DateTime StartDate, DateTime EndDate, int Skip = 0, int Take = 100)
{
    public ReportRequest(int skip = 0, int take = 100) : this(DateTime.MinValue.ToUniversalTime(), DateTime.UtcNow,
        skip, take)
    {
    }
}