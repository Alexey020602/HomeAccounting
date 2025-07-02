namespace Receipts.Core.AddReceipt.BarCode;

public static class StreamExtensions
{
    public static async Task<byte[]> ReadAsByteArrayAsync(this Stream stream,
        CancellationToken cancellationToken = default)
    {
        var memory = new byte[stream.Length];
        await stream.ReadExactlyAsync(memory, 0, memory.Length, cancellationToken);
        return memory;
    }
}