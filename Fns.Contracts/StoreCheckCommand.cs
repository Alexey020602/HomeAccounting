namespace Fns.Contracts;

public record StoreCheckCommand(
    string Login,
    DateTime PurchaseDate,
    string Fn,
    string Fd,
    string Fp,
    string Sum,
    IReadOnlyList<StoreCheckCommand.Product> Products
)
{
    public record Product(
        string Name,
        double Quantity,
        int Price,
        int Sum,
        string? Subcategory,
        string Category
    );
}