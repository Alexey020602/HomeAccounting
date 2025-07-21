using Receipts.Contracts;
using Receipts.Core.Model;

namespace Receipts.Core.GetReceipts;

//todo переделать репозиторий на классы Core
public interface IGetReceiptsService
{
    Task<IReadOnlyList<Check>> GetChecksAsync(Contracts.GetChecks getChecksQuery);
}

public record GetCheckRequest(ReceiptData ReceiptData);

public record AddCheckRequest(ReceiptData ReceiptData, IReadOnlyList<AddCheckRequest.Product> Products)
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
