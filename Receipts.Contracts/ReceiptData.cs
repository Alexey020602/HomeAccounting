using System.Text.Json.Serialization;
using Fns.Contracts;

namespace Receipts.Contracts;

[method:JsonConstructor]
public record ReceiptData(ReceiptFiscalData FiscalData, Guid UserId, Guid BudgetId);