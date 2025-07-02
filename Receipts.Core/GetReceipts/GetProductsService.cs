using Receipts.Contracts;
using Receipts.Core.Mappers;

namespace Receipts.Core.GetReceipts;

public class GetProductsService(IGetReceiptsService receiptsService): IGetProductsService
{
    public async Task<IReadOnlyList<Category>> GetProductsAsync(Contracts.GetChecks getChecksQuery) =>
        (await receiptsService.GetChecksAsync(getChecksQuery))
        .SelectMany(check => check.Products)
        .ConvertToCategories();
}
