namespace Receipts.Contracts;

public class SaveCheckRequest
{
    public required string Login { get; init; }
    public required string Fn { get; init; }
    public required string Fd { get; init; }
    public required string Fp { get; init; }
    public required string S  { get; init; }
    public required DateTime T { get; init; }
}