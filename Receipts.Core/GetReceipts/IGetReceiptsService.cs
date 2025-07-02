using Receipts.Core.Model;

namespace Receipts.Core.GetReceipts;

//todo переделать репозиторий на классы Core
public interface IGetReceiptsService
{
    Task<IReadOnlyList<Check>> GetChecksAsync(Contracts.GetChecks getChecksQuery);
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
