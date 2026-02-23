using HomeAccounting.Common.Model;

namespace HomeAccounting.ReceiptProcessing.Contracts;

internal sealed record GetReceiptDataRequest(ReceiptFiscalData ReceiptFiscalData);