namespace ClientServerShared.QrCode;

public static class QrCodeServiceExtensions
{
    public static async Task<string> ReadBarcodeAsync(
        this IQrCodeReader qrCodeReader,
        byte[] imageBytes)
    {
        return await qrCodeReader.ReadQrCodeAsync(new MemoryStream(imageBytes));
    }
}