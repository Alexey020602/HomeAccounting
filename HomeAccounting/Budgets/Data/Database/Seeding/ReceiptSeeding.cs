using ClientServerShared.Model.Money;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;

namespace HomeAccounting.Budgets.Data;

partial class Receipt
{
    public static IEnumerable<Receipt> GetDefaultReceiptsForFirstBudget()
    {
        var budgetId = new BudgetId(Budget.FirstBudgetId);
        var userId = new UserId(UserConstants.DefaultUserId);
        var addedDate = new DateTimeOffset(2024, 10, 15, 12, 0, 0, TimeSpan.Zero);
        var fiscalData = ReceiptFiscalData.Create(
            "9282440300123456",
            "12345",
            "1234567890",
            Money.FromRubles(1250.50m),
            new DateTimeOffset(2024, 10, 15, 11, 55, 0, TimeSpan.Zero));
        var products = new[]
        {
            new ProductInput("Молоко 3.2%", 2, Money.FromRubles(85.00m), Money.FromRubles(170.00m), null),
            new ProductInput("Хлеб белый", 1, Money.FromRubles(45.50m), Money.FromRubles(45.50m), null),
            new ProductInput("Сыр", 0.5, Money.FromRubles(600.00m), Money.FromRubles(300.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-1111-759b-b91a-1b51403e8039")),
            budgetId,
            addedDate,
            fiscalData,
            userId,
            "Магнит",
            products);
    }

    public static IEnumerable<Receipt> GetDefaultReceiptsForSecondBudget()
    {
        var budgetId = new BudgetId(Budget.SecondBudgetId);
        var userId = new UserId(UserConstants.DefaultUserId);
        var addedDate = new DateTimeOffset(2024, 10, 20, 19, 0, 0, TimeSpan.Zero);
        var fiscalData = ReceiptFiscalData.Create(
            "9282440300999999",
            "54321",
            "9876543210",
            Money.FromRubles(890.00m),
            new DateTimeOffset(2024, 10, 20, 18, 50, 0, TimeSpan.Zero));
        var products = new[]
        {
            new ProductInput("Кофе зерновой", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Печенье", 1, Money.FromRubles(440.00m), Money.FromRubles(440.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2222-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate,
            fiscalData,
            userId,
            "Пятёрочка",
            products);
    }
}
