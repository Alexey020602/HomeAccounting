namespace Fns.Contracts.Categorization;

public record NormalizedCheck(
    DateTime PurchaseDate,
    string Fd,
    string Fn,
    string Fp,
    string Sum,
    IReadOnlyList<NormalizedProduct> Products
);