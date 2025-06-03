using Shared.Model;
using Shared.Model.Checks;

namespace Checks.Core;

public interface ICheckRepository
{
    Task<Check> SaveCheck(AddCheckRequest request);

    Task<Check?> GetCheckByRequest(GetCheckRequest checkRequest);
    Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}

public class GetCheckRequest(string fn, string fd, string fp, string s, DateTime t)
{
    public string Fn { get; } = fn;
    public string Fd { get; } = fd;
    public string Fp { get; } = fp;
    public string S { get; } = s;
    public DateTime T { get; } = t;
}

public record AddCheckRequest(User User, DateTime PurchaseDate, string Fn, string Fd, string Fp, string S, IReadOnlyList<AddCheckRequest.Product> Products)
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
