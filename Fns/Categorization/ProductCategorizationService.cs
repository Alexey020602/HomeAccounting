using Fns.Categorization.CheiCheck.Dto;
using Fns.Contracts;
using Fns.Contracts.Categorization;

namespace Fns.Categorization;

sealed class ProductCategorizationService(IReceiptService receiptService) : IProductCategorizationService
{
    public async Task<CategorizationResponse> CategorizeProducts(CategorizationRequest request)
    {
        var query = new Query(request.Products.Select(p => p.Name).ToList());
        var response = await receiptService.GetReceipt(query);
        return new CategorizationResponse(Products: response.Items.Select(item =>
            new CategorizedProduct(
                item.InitialRequest, 
                item.Category.SecondLevelCategory,
                item.Category.FirstLevelCategory)
        ).ToList());
    }
}