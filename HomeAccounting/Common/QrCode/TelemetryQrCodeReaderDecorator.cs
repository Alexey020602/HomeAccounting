using System.Diagnostics;
using ClientServerShared.QrCode;

namespace HomeAccounting.Common.QrCode;

public sealed class TelemetryQrCodeReaderDecorator(
    IQrCodeReader qrCodeReader,
    ILogger<TelemetryQrCodeReaderDecorator> logger,
    ActivitySource activitySource
) : IQrCodeReader
{
    public async ValueTask<string> ReadQrCodeAsync(Stream stream)
    {
        using var activity = activitySource.StartActivity();
        logger.LogInformation("Reading QR code {ActivityId}",  activity?.Id);
        try
        {
            var result = await qrCodeReader.ReadQrCodeAsync(stream);
            activity?.SetTag("barcode", result);
            logger.LogInformation("QR code read complete");
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception e)
        {
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            activity?.AddException(e);
            logger.LogError(e, "Error while reading QR code");
            throw;
        }
    }
}
