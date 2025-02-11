namespace Core.Model.Report;

public record ReportRequest(DateTime StartDate, DateTime EndDate)
{
    public ReportRequest() : this(DateTime.MinValue.ToUniversalTime(), DateTime.UtcNow)
    {
        
    }
}