using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;

namespace HomeAccounting.Budgets.Data;

partial class Budget
{
    public static readonly Guid FirstBudgetId = Guid.Parse("01989e7d-5251-759b-b91a-1b51403e8039");
    public static readonly Guid SecondBudgetId = Guid.Parse("01989e7d-8b91-75e8-91a1-7edc1f9b3385");

    public static IEnumerable<Budget> GetDefaultBudgets() =>
    [
        new (
            "Мой и Сашин бюджет",
            1,
            null,
            new UserId(UserConstants.DefaultUserId),
            DateTimeOffset.UtcNow,
            [
                new BudgetUser(new UserId(UserConstants.DefaultUserId), BudgetRole.OwnerBudgetRoleId),
                new BudgetUser(new UserId(UserConstants.SecondUserId), BudgetRole.AdminBudgetRoleId)
            ])
        {
            Id = new BudgetId(FirstBudgetId),
            operations =
            [
                .. Operation.GetDefaultOperationsForFirstBudget()
            ],
        },
        new (
            "Личный бюджет",
            7,
            Money.FromRubles(15000.00m),
            new UserId(UserConstants.DefaultUserId),
            DateTimeOffset.UtcNow,
            [
                new BudgetUser(new UserId(UserConstants.DefaultUserId), BudgetRole.OwnerBudgetRoleId),
            ])
        {
            Id = new BudgetId(SecondBudgetId),
            operations =
            [
                .. Operation.GetDefaultOperationsForSecondBudget()
            ],
        },
    ];
}

partial class Operation
{
    public static IEnumerable<Operation> GetDefaultOperationsForFirstBudget() =>
    [
        new Operation(
            Money.FromRubles(3500.00m),
            new DateTimeOffset(2024, 10, 5, 14, 30, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 10, 5, 15, 0, 0, TimeSpan.Zero),
            "Оплата интернета",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(2800.00m),
            new DateTimeOffset(2024, 10, 12, 10, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 10, 12, 10, 15, 0, TimeSpan.Zero),
            "Оплата мобильной связи",
            new UserId(UserConstants.SecondUserId)
        ),
        new Operation(
            Money.FromRubles(12000.00m),
            new DateTimeOffset(2024, 10, 20, 18, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 10, 20, 18, 30, 0, TimeSpan.Zero),
            "Поход в кино",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(4500.00m),
            new DateTimeOffset(2024, 11, 3, 12, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 3, 12, 20, 0, TimeSpan.Zero),
            "Обед в ресторане",
            new UserId(UserConstants.SecondUserId)
        ),
        new Operation(
            Money.FromRubles(8500.00m),
            new DateTimeOffset(2024, 11, 15, 16, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 15, 16, 45, 0, TimeSpan.Zero),
            "Подарок на день рождения",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(2200.00m),
            new DateTimeOffset(2024, 11, 25, 9, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 25, 9, 10, 0, TimeSpan.Zero),
            "Такси",
            new UserId(UserConstants.SecondUserId)
        ),
        new Operation(
            Money.FromRubles(15000.00m),
            new DateTimeOffset(2024, 12, 10, 19, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 12, 10, 19, 30, 0, TimeSpan.Zero),
            "Новогодние подарки",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(3200.00m),
            new DateTimeOffset(2024, 12, 20, 11, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 12, 20, 11, 15, 0, TimeSpan.Zero),
            "Оплата подписки на стриминг",
            new UserId(UserConstants.SecondUserId)
        ),
        new Operation(
            Money.FromRubles(6800.00m),
            new DateTimeOffset(2025, 1, 5, 13, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2025, 1, 5, 13, 20, 0, TimeSpan.Zero),
            "Обед в кафе",
            new UserId(UserConstants.DefaultUserId)
        ),
    ];

    public static IEnumerable<Operation> GetDefaultOperationsForSecondBudget() =>
    [
        new Operation(
            Money.FromRubles(2500.00m),
            new DateTimeOffset(2024, 10, 8, 8, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 10, 8, 8, 10, 0, TimeSpan.Zero),
            "Оплата проездного",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(1800.00m),
            new DateTimeOffset(2024, 10, 15, 20, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 10, 15, 20, 15, 0, TimeSpan.Zero),
            "Кофе и завтрак",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(4200.00m),
            new DateTimeOffset(2024, 11, 1, 17, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 1, 17, 30, 0, TimeSpan.Zero),
            "Книги",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(9500.00m),
            new DateTimeOffset(2024, 11, 10, 15, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 10, 15, 45, 0, TimeSpan.Zero),
            "Одежда",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(1200.00m),
            new DateTimeOffset(2024, 11, 22, 12, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 11, 22, 12, 10, 0, TimeSpan.Zero),
            "Обед",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(3500.00m),
            new DateTimeOffset(2024, 12, 5, 10, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 12, 5, 10, 20, 0, TimeSpan.Zero),
            "Фитнес-абонемент",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(2800.00m),
            new DateTimeOffset(2024, 12, 18, 14, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2024, 12, 18, 14, 15, 0, TimeSpan.Zero),
            "Косметика",
            new UserId(UserConstants.DefaultUserId)
        ),
        new Operation(
            Money.FromRubles(1500.00m),
            new DateTimeOffset(2025, 1, 3, 9, 0, 0, TimeSpan.Zero),
            null,
            new DateTimeOffset(2025, 1, 3, 9, 10, 0, TimeSpan.Zero),
            "Завтрак",
            new UserId(UserConstants.DefaultUserId)
        ),
    ];
}
