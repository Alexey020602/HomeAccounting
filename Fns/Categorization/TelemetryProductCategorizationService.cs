using System.Diagnostics;
using Fns.Contracts.Categorization;
using Microsoft.Extensions.Logging;

namespace Fns.Categorization;

sealed class TelemetryProductCategorizationService(
    ILogger<TelemetryProductCategorizationService> logger, 
    ActivitySource activitySource, 
    IProductCategorizationService productCategorizationService)
    : IProductCategorizationService
{
    public async Task<CategorizationResponse> CategorizeProducts(CategorizationRequest request)
    {
        
        using var activity = activitySource.StartActivity("Get Receipt Categories");
        activity?.SetTag("Products", QueryDescription(request));
        logger.LogInformation("Get Receipt Categories");
        
        try
        {
            var result = await productCategorizationService.CategorizeProducts(request);
            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation("Get Receipt Categories");
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error,  ex.Message);
            activity?.AddException(ex);
            logger.LogError(ex, "Error getting Receipt Categories");
            throw;
        }
    }
    private static string QueryDescription(CategorizationRequest query) => string.Join(
        "\n", 
        query.Products.Select(p => p.Name));
}