using System.Diagnostics;
using ClientServerShared.BarCode;

namespace HomeAccounting.Common;

public sealed class TelemetryBarcodeServiceDecorator(
    IBarcodeService barcodeService,
    ILogger<TelemetryBarcodeServiceDecorator> logger,
    ActivitySource activitySource
) : IBarcodeService
{
    public async ValueTask<string> ReadBarcodeAsync(Stream stream)
    {
        using var activity = activitySource.StartActivity();
        logger.LogInformation("Reading barcode {ActivityId}",  activity?.Id);
        try
        {
            var result = await barcodeService.ReadBarcodeAsync(stream);
            activity?.SetTag("barcode", result);
            logger.LogInformation("Barcode read complete");
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception e)
        {
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            activity?.AddException(e);
            logger.LogError(e, "Error while reading barcode");
            throw;
        }
    }

    public async ValueTask<string> ReadBarcodeAsync(byte[] imageBytes)
    {
        using var activity = activitySource.StartActivity();
        logger.LogInformation("Reading barcode {ActivityId}",  activity?.Id);
        try
        {
            var result = await barcodeService.ReadBarcodeAsync(imageBytes);
            activity?.SetTag("barcode", result);
            logger.LogInformation("Barcode read complete");
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception e)
        {
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            activity?.AddException(e);
            logger.LogError(e, "Error while reading barcode");
            throw;
        }
    }
}
