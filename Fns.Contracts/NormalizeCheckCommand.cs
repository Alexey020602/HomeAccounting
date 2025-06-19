namespace Fns;

public record NormalizeCheckCommand
{
    public required string Fn { get; init; }
    public required string Fd { get; init; }
    public required string Fp { get; init; }
    public required string S { get; init; }
    public required DateTime T { get; init; }
    // public required DateTime AddedDate { get; init; }
    public required string Login { get; init; }
    // public required double Quantity { get; init; }
    // public required int Price { get; init; }
    // public required int Sum { get; init; }
    public required IReadOnlyList<Product> Products { get; init; }
    public record Product(string Name, double Quantity, int Price, int Sum);
}