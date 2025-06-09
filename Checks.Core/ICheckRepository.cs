using Authorization.Contracts;
using Checks.Contracts;
using Checks.Core.Model;
using Shared.Model;
using Shared.Model.Checks;
using Category = Checks.Contracts.Category;
using Product = Checks.Core.Model.Product;

namespace Checks.Core;
//todo переделать репозиторий на классы Core
public interface ICheckRepository
{
    Task<Check> SaveCheck(AddCheckRequest request);

    Task<Check?> GetCheckByRequest(GetCheckRequest checkRequest);
    Task<IReadOnlyList<Check>> GetChecksAsync(GetChecksQuery getChecksQuery);
    Task<IReadOnlyList<Product>> GetProductsAsync(GetChecksQuery getChecksQuery);
}

public class GetCheckRequest(string fn, string fd, string fp, string s, DateTime t)
{
    public string Fn { get; } = fn;
    public string Fd { get; } = fd;
    public string Fp { get; } = fp;
    public string S { get; } = s;
    public DateTime T { get; } = t;
}

public record AddCheckRequest(string Login, DateTime PurchaseDate, string Fn, string Fd, string Fp, string S, IReadOnlyList<AddCheckRequest.Product> Products)
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
