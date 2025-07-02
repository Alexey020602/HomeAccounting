namespace Receipts.Contracts;

public interface IGetProductsService
{
    Task<IReadOnlyList<Category>> GetProductsAsync(GetChecks getChecksQuery);
}