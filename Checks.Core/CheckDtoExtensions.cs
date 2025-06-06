using Authorization.Contracts;
using Shared.Model;
using CheckDto = Fns.Contracts.NormalizedCheck;
using ProductDto = Fns.Contracts.NormalizedProduct;
namespace Checks.Core;

static class CheckDtoExtensions
{
    public static AddCheckRequest CreateFromCheckDto(this CheckDto check, string login) => new(
        login,
        check.PurchaseDate,
        check.Fn,
        check.Fd,
        check.Fp,
        check.Sum,
        check.Products.Select(ConvertToAddCheckRequestProduct).ToList());

    private static AddCheckRequest.Product ConvertToAddCheckRequestProduct(this ProductDto product) =>
        new AddCheckRequest.Product(
            product.Name,
            product.Quantity,
            product.Price,
            product.Sum,
            product.Subcategory,
            product.Category);
}