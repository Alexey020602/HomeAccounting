using Microsoft.Extensions.Options;

namespace HomeAccounting.Budgets.Configuration;

internal sealed class ReceiptProcessingOptionsValidator : IValidateOptions<ReceiptProcessingOptions>
{
    public ValidateOptionsResult Validate(string? name, ReceiptProcessingOptions options)
    {
        var errors = new List<string>();
        
        if (options.MaxRetries < 1)
        {
            errors.Add($"{nameof(ReceiptProcessingOptions.MaxRetries)} must be >= 1, but was {options.MaxRetries}");
        }
        
        if (options.BackoffMultiplier <= 1.0)
        {
            errors.Add($"{nameof(ReceiptProcessingOptions.BackoffMultiplier)} must be > 1.0, but was {options.BackoffMultiplier}");
        }
        
        if (options.InitialDelay <= TimeSpan.Zero)
        {
            errors.Add($"{nameof(ReceiptProcessingOptions.InitialDelay)} must be > 0, but was {options.InitialDelay}");
        }
        
        if (options.MaxDelay <= TimeSpan.Zero)
        {
            errors.Add($"{nameof(ReceiptProcessingOptions.MaxDelay)} must be > 0, but was {options.MaxDelay}");
        }
        
        if (options.MaxDelay <= options.InitialDelay)
        {
            errors.Add($"{nameof(ReceiptProcessingOptions.MaxDelay)} ({options.MaxDelay}) must be greater than {nameof(ReceiptProcessingOptions.InitialDelay)} ({options.InitialDelay})");
        }
        
        return errors.Count > 0 
            ? ValidateOptionsResult.Fail(errors) 
            : ValidateOptionsResult.Success;
    }
}
