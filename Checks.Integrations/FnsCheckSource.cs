using Checks.Core;
using Shared.Model.NormalizedChecks;
using ICheckSource = Checks.Core.ICheckSource;
using IFnsCheckSource = Shared.Model.NormalizedChecks.ICheckSource;
namespace Checks.Integrations;

sealed class FnsCheckSource(IFnsCheckSource checkSource): ICheckSource
{
    private readonly IFnsCheckSource checkSource = checkSource;
    public async Task<CheckDto> GetCheck(GetCheckRequest request) =>
        (await checkSource.GetCheck(request.CreateFromSaveCheckRequest())).ConvertToCheckDto();
}

static class CheckRequestExtensions
{
    public static Shared.Model.Requests.CheckRequest CreateFromSaveCheckRequest(this GetCheckRequest saveCheckRequest) => new(
        saveCheckRequest.Fn,
        saveCheckRequest.Fd,
        saveCheckRequest.Fp,
        saveCheckRequest.S,
        saveCheckRequest.T
    );

    public static CheckDto ConvertToCheckDto(this NormalizedCheck check) => new(
        check.PurchaseDate, 
        check.Fd, 
        check.Fn,
        check.Fp,
        check.Sum,
        check.Products.Select(ConvertToProductDto).ToList());
    private static ProductDto ConvertToProductDto(this NormalizedProduct product) => new(
        product.Name, 
        product.Quantity, 
        product.Price, 
        product.Sum, 
        product.Subcategory,
        product.Category);
}