using Shared.Model.NormalizedChecks;

namespace Checks.Core;

public interface ICheckSource
{
    public Task<CheckDto> GetCheck(GetCheckRequest request);
}

public record CheckDto(
    DateTime PurchaseDate,
    string Fd,
    string Fn,
    string Fp,
    string Sum,
    IReadOnlyList<ProductDto> Products)
{
    public CheckDto(NormalizedCheck check) : this(
        check.PurchaseDate, 
        check.Fd, 
        check.Fn,
        check.Fp, 
        check.Sum,
        check.Products.Select(p => new ProductDto(p)).ToList())
    {
    }
};

public record ProductDto(string Name, double Quantity, int Price, int Sum, string? Subcategory, string Category)
{
    public ProductDto(NormalizedProduct product) : this(
        product.Name, 
        product.Quantity, 
        product.Price, 
        product.Sum,
        product.Subcategory, 
        product.Category) { }
}