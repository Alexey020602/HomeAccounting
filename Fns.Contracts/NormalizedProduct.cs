namespace Fns.Contracts;

public record NormalizedProduct(
    string Name,
    double Quantity,
    int Price,
    int Sum,
    string? Subcategory,
    string Category
);