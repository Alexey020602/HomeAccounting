using FnsChecksApi.Dto.Categorized;
using Refit;

namespace FnsChecksApi;

public interface IReceiptService
{
    [Post("/api/receipt")]
    Task<Root> GetReceipt(Query query);
}