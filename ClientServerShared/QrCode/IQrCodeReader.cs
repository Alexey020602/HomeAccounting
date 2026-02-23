namespace ClientServerShared.QrCode;

public interface IQrCodeReader
{
    ValueTask<string> ReadQrCodeAsync(Stream stream);
}