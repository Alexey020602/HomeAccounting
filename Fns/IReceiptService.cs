using Fns.CheiCheck.Dto;
using Refit;

namespace Fns;

public interface IReceiptService
{
    [Post("/api/receipt")]
    Task<Root> GetReceipt(Query query);
}