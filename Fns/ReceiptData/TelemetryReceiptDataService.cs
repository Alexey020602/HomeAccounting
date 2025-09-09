using System.Diagnostics;
using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using MaybeResults;
using Microsoft.Extensions.Logging;

namespace Fns.ReceiptData;

sealed class TelemetryReceiptDataService(
    IReceiptDataService receiptDataService,
    ILogger<TelemetryReceiptDataService> logger,
    ActivitySource activitySource
    ): IReceiptDataService
{
    public async Task<IMaybe<Receipt>> GetReceipt(ReceiptFiscalData receiptFiscalData)
    {
        using var activity = activitySource.StartActivity();
        activity?.SetTag("ReceiptRaw", receiptFiscalData.RawCheck());
        logger.LogInformation("Getting receipt data with raw data {RawCheck}", receiptFiscalData.RawCheck());
        try
        {
            var result = await receiptDataService.GetReceipt(receiptFiscalData);
            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation("Receipt data gotten");
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
            logger.LogError(ex, "Error getting receipt data");
            throw;
        }
    }
}