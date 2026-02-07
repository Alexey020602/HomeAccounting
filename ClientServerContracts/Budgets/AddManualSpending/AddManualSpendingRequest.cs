namespace ClientServerContracts.Budgets.AddManualSpending;

public record AddManualSpendingRequest(int Sum, string Description, DateTimeOffset PurchaseDate, int? CategoryId);







