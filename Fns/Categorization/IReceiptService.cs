using Fns.Categorization.CheiCheck.Dto;
using Refit;

namespace Fns.Categorization;

public interface IReceiptService
{
    [Post("/api/receipt")]
    Task<Root> GetReceipt(Query query);
}