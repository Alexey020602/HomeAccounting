namespace ClientServerShared.BarCode;

public interface IBarcodeService
{
    public ValueTask<string> ReadBarcodeAsync(byte[] imageBytes);
    ValueTask<string> ReadBarcodeAsync(Stream stream);
}