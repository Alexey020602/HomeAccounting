namespace ClientServerContracts.Budgets.AddReceiptSpending;

public record AddReceiptSpendingRequest(string Fn, string Fd, string Fp, int Sum, DateTimeOffset PurchaseDate);
