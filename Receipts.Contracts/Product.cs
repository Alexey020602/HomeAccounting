using Shared.Model;

namespace Receipts.Contracts;

public class Product
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required double Quantity { get; init; }
    public required int PennySum { get; init; }
    public required int Price { get; init; }
    public Sum Sum => new(PennySum);
}