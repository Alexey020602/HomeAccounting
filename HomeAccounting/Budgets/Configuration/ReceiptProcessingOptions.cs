namespace HomeAccounting.Budgets.Configuration;

internal sealed class ReceiptProcessingOptions
{
    public const string SectionName = "ReceiptProcessing";
    
    public int MaxRetries { get; set; } = 5;
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(5);
    public double BackoffMultiplier { get; set; } = 2.0;
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromMinutes(30);
}
