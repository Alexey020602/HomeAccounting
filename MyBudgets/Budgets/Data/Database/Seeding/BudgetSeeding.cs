using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using MyBudgets.Common.Model;
using MyBudgets.Users.Data;
using MyBudgets.Users.Data.Database;

namespace MyBudgets.Budgets.Data;

partial class Budget
{
    public static readonly Guid FirstBudgetId = Guid.Parse("01989e7d-5251-759b-b91a-1b51403e8039");
    public static readonly Guid SecondBudgetId = Guid.Parse("01989e7d-8b91-75e8-91a1-7edc1f9b3385");
    // public const int FirstBudgetId = 1;
    // public const int SecondBudgetId = 2;
    public static IEnumerable<Budget> GetDefaultBudgets() =>
    [
        new (
            "Мой и Сашин бюджет", 
            1, 
            null, 
            new UserId(UserConstants.DefaultUserId), 
            DateTime.UtcNow, 
            [
                new BudgetUser(new UserId(UserConstants.DefaultUserId), BudgetRole.OwnerBudgetRoleId),
                new BudgetUser(new UserId(UserConstants.SecondUserId), BudgetRole.AdminBudgetRoleId)
            ])
        {
            Id = new (FirstBudgetId),
            spendings = [
                .. ReceiptSpending.GetDefaultReceiptSpendingsForFirstBudget(), 
                .. ManualSpending.GetDefaultManualSpendingsForFirstBudget()
            ],
        },
        new (
            "Личный бюджет",
            7,
            Money.FromRubles(15000.00m),
            new UserId(UserConstants.DefaultUserId),
            DateTime.UtcNow,
            [
                new BudgetUser(new UserId(UserConstants.DefaultUserId), BudgetRole.OwnerBudgetRoleId),
            ])
        {
            Id = new (SecondBudgetId),
            spendings = [
                .. ReceiptSpending.GetDefaultReceiptSpendingsForSecondBudget(), 
                .. ManualSpending.GetDefaultManualSpendingsForSecondBudget()
            ],
        },
    ];
}

partial class ManualSpending
{
    public static IEnumerable<ManualSpending> GetDefaultManualSpendingsForFirstBudget() => 
    [
        new ManualSpending(
            Money.FromRubles(3500.00m),
            new DateTime(2024, 10, 5, 14, 30, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 10, 5, 15, 0, 0, DateTimeKind.Utc),
            "Оплата интернета",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(2800.00m),
            new DateTime(2024, 10, 12, 10, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 10, 12, 10, 15, 0, DateTimeKind.Utc),
            "Оплата мобильной связи",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            Money.FromRubles(12000.00m),
            new DateTime(2024, 10, 20, 18, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 10, 20, 18, 30, 0, DateTimeKind.Utc),
            "Поход в кино",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(4500.00m),
            new DateTime(2024, 11, 3, 12, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 3, 12, 20, 0, DateTimeKind.Utc),
            "Обед в ресторане",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            Money.FromRubles(8500.00m),
            new DateTime(2024, 11, 15, 16, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 15, 16, 45, 0, DateTimeKind.Utc),
            "Подарок на день рождения",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(2200.00m),
            new DateTime(2024, 11, 25, 9, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 25, 9, 10, 0, DateTimeKind.Utc),
            "Такси",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            Money.FromRubles(15000.00m),
            new DateTime(2024, 12, 10, 19, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 12, 10, 19, 30, 0, DateTimeKind.Utc),
            "Новогодние подарки",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(3200.00m),
            new DateTime(2024, 12, 20, 11, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 12, 20, 11, 15, 0, DateTimeKind.Utc),
            "Оплата подписки на стриминг",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            Money.FromRubles(6800.00m),
            new DateTime(2025, 1, 5, 13, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2025, 1, 5, 13, 20, 0, DateTimeKind.Utc),
            "Обед в кафе",
            new UserId(UserConstants.DefaultUserId)
        ),
    ];

    public static IEnumerable<ManualSpending> GetDefaultManualSpendingsForSecondBudget() =>
    [
        new ManualSpending(
            Money.FromRubles(2500.00m),
            new DateTime(2024, 10, 8, 8, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 10, 8, 8, 10, 0, DateTimeKind.Utc),
            "Оплата проездного",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(1800.00m),
            new DateTime(2024, 10, 15, 20, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 10, 15, 20, 15, 0, DateTimeKind.Utc),
            "Кофе и завтрак",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(4200.00m),
            new DateTime(2024, 11, 1, 17, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 1, 17, 30, 0, DateTimeKind.Utc),
            "Книги",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(9500.00m),
            new DateTime(2024, 11, 10, 15, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 10, 15, 45, 0, DateTimeKind.Utc),
            "Одежда",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(1200.00m),
            new DateTime(2024, 11, 22, 12, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 11, 22, 12, 10, 0, DateTimeKind.Utc),
            "Обед",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(3500.00m),
            new DateTime(2024, 12, 5, 10, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 12, 5, 10, 20, 0, DateTimeKind.Utc),
            "Фитнес-абонемент",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(2800.00m),
            new DateTime(2024, 12, 18, 14, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2024, 12, 18, 14, 15, 0, DateTimeKind.Utc),
            "Косметика",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            Money.FromRubles(1500.00m),
            new DateTime(2025, 1, 3, 9, 0, 0, DateTimeKind.Utc),
            null,
            new DateTime(2025, 1, 3, 9, 10, 0, DateTimeKind.Utc),
            "Завтрак",
            new UserId(UserConstants.DefaultUserId)
        ),
    ];
}

partial class ReceiptSpending
{
    public static IEnumerable<ReceiptSpending> GetDefaultReceiptSpendingsForFirstBudget() =>
    [
        new ReceiptSpending(
            new DateTime(2024, 10, 7, 18, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 7, 19, 0, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256789", "12345", "6789012345"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Хлеб белый", 1, Money.FromRubles(65.00m), Money.FromRubles(65.00m), null),
                new ProductInput("Молоко 3.2%", 1, Money.FromRubles(89.00m), Money.FromRubles(89.00m), null),
                new ProductInput("Яйца куриные С0", 1, Money.FromRubles(125.00m), Money.FromRubles(125.00m), null),
                new ProductInput("Сыр Российский", 0.5, Money.FromRubles(450.00m), Money.FromRubles(225.00m), null),
                new ProductInput("Помидоры", 0.8, Money.FromRubles(180.00m), Money.FromRubles(144.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 14, 19, 15, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 14, 19, 45, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256790", "12346", "6789012346"),
            new UserId(UserConstants.SecondUserId),
            "Магнит",
            [
                new ProductInput("Курица охлажденная", 1.2, Money.FromRubles(350.00m), Money.FromRubles(420.00m), null),
                new ProductInput("Картофель", 2.0, Money.FromRubles(45.00m), Money.FromRubles(90.00m), null),
                new ProductInput("Лук репчатый", 0.5, Money.FromRubles(60.00m), Money.FromRubles(30.00m), null),
                new ProductInput("Морковь", 0.6, Money.FromRubles(55.00m), Money.FromRubles(33.00m), null),
                new ProductInput("Масло подсолнечное", 1, Money.FromRubles(185.00m), Money.FromRubles(185.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 21, 17, 45, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 21, 18, 15, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256791", "12347", "6789012347"),
            new UserId(UserConstants.DefaultUserId),
            "Перекресток",
            [
                new ProductInput("Говядина вырезка", 0.8, Money.FromRubles(650.00m), Money.FromRubles(520.00m), null),
                new ProductInput("Сметана 20%", 1, Money.FromRubles(95.00m), Money.FromRubles(95.00m), null),
                new ProductInput("Творог 5%", 0.4, Money.FromRubles(180.00m), Money.FromRubles(72.00m), null),
                new ProductInput("Бананы", 1.5, Money.FromRubles(120.00m), Money.FromRubles(180.00m), null),
                new ProductInput("Яблоки", 1.0, Money.FromRubles(140.00m), Money.FromRubles(140.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 5, 16, 20, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 5, 16, 50, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256792", "12348", "6789012348"),
            new UserId(UserConstants.SecondUserId),
            "Лента",
            [
                new ProductInput("Рыба семга", 0.5, Money.FromRubles(1200.00m), Money.FromRubles(600.00m), null),
                new ProductInput("Рис", 1, Money.FromRubles(95.00m), Money.FromRubles(95.00m), null),
                new ProductInput("Огурцы", 0.7, Money.FromRubles(150.00m), Money.FromRubles(105.00m), null),
                new ProductInput("Салат листовой", 1, Money.FromRubles(120.00m), Money.FromRubles(120.00m), null),
                new ProductInput("Оливковое масло", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 18, 20, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 18, 20, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256793", "12349", "6789012349"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Колбаса докторская", 0.3, Money.FromRubles(450.00m), Money.FromRubles(135.00m), null),
                new ProductInput("Хлеб ржаной", 1, Money.FromRubles(55.00m), Money.FromRubles(55.00m), null),
                new ProductInput("Масло сливочное", 0.2, Money.FromRubles(550.00m), Money.FromRubles(110.00m), null),
                new ProductInput("Чай черный", 1, Money.FromRubles(180.00m), Money.FromRubles(180.00m), null),
                new ProductInput("Кофе молотый", 1, Money.FromRubles(320.00m), Money.FromRubles(320.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 8, 15, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 8, 16, 0, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256794", "12350", "6789012350"),
            new UserId(UserConstants.SecondUserId),
            "Семишагов",
            [
                new ProductInput("Шампанское", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
                new ProductInput("Сыр пармезан", 0.2, Money.FromRubles(1200.00m), Money.FromRubles(240.00m), null),
                new ProductInput("Ветчина", 0.3, Money.FromRubles(550.00m), Money.FromRubles(165.00m), null),
                new ProductInput("Икра красная", 1, Money.FromRubles(850.00m), Money.FromRubles(850.00m), null),
                new ProductInput("Хлеб белый нарезка", 1, Money.FromRubles(75.00m), Money.FromRubles(75.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 22, 18, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 22, 18, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256795", "12351", "6789012351"),
            new UserId(UserConstants.DefaultUserId),
            "Ашан",
            [
                new ProductInput("Мандарины", 2.0, Money.FromRubles(120.00m), Money.FromRubles(240.00m), null),
                new ProductInput("Шоколад", 3, Money.FromRubles(95.00m), Money.FromRubles(285.00m), null),
                new ProductInput("Печенье", 2, Money.FromRubles(85.00m), Money.FromRubles(170.00m), null),
                new ProductInput("Сок апельсиновый", 2, Money.FromRubles(120.00m), Money.FromRubles(240.00m), null),
                new ProductInput("Йогурт", 4, Money.FromRubles(65.00m), Money.FromRubles(260.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2025, 1, 8, 17, 15, 0, DateTimeKind.Utc),
            new DateTime(2025, 1, 8, 17, 45, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256796", "12352", "6789012352"),
            new UserId(UserConstants.SecondUserId),
            "Пятерочка",
            [
                new ProductInput("Курица целая", 1.5, Money.FromRubles(280.00m), Money.FromRubles(420.00m), null),
                new ProductInput("Гречка", 1, Money.FromRubles(85.00m), Money.FromRubles(85.00m), null),
                new ProductInput("Лук зеленый", 1, Money.FromRubles(90.00m), Money.FromRubles(90.00m), null),
                new ProductInput("Укроп", 1, Money.FromRubles(75.00m), Money.FromRubles(75.00m), null),
                new ProductInput("Сметана 15%", 1, Money.FromRubles(85.00m), Money.FromRubles(85.00m), null),
            ]
        ),
    ];

    public static IEnumerable<ReceiptSpending> GetDefaultReceiptSpendingsForSecondBudget() =>
    [
        new ReceiptSpending(
            new DateTime(2024, 10, 10, 19, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 10, 19, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256800", "12400", "6789012400"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Хлеб белый", 1, Money.FromRubles(65.00m), Money.FromRubles(65.00m), null),
                new ProductInput("Молоко 2.5%", 1, Money.FromRubles(79.00m), Money.FromRubles(79.00m), null),
                new ProductInput("Йогурт", 2, Money.FromRubles(65.00m), Money.FromRubles(130.00m), null),
                new ProductInput("Банан", 1.2, Money.FromRubles(120.00m), Money.FromRubles(144.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 18, 18, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 18, 19, 0, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256801", "12401", "6789012401"),
            new UserId(UserConstants.DefaultUserId),
            "Магнит",
            [
                new ProductInput("Куриная грудка", 0.6, Money.FromRubles(380.00m), Money.FromRubles(228.00m), null),
                new ProductInput("Рис", 1, Money.FromRubles(95.00m), Money.FromRubles(95.00m), null),
                new ProductInput("Брокколи", 0.4, Money.FromRubles(200.00m), Money.FromRubles(80.00m), null),
                new ProductInput("Морковь", 0.5, Money.FromRubles(55.00m), Money.FromRubles(28.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 5, 17, 45, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 5, 18, 15, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256802", "12402", "6789012402"),
            new UserId(UserConstants.DefaultUserId),
            "Перекресток",
            [
                new ProductInput("Лосось", 0.4, Money.FromRubles(950.00m), Money.FromRubles(380.00m), null),
                new ProductInput("Авокадо", 2, Money.FromRubles(180.00m), Money.FromRubles(360.00m), null),
                new ProductInput("Салат айсберг", 1, Money.FromRubles(150.00m), Money.FromRubles(150.00m), null),
                new ProductInput("Оливковое масло", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 15, 16, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 15, 16, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256803", "12403", "6789012403"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Творог 9%", 0.4, Money.FromRubles(200.00m), Money.FromRubles(80.00m), null),
                new ProductInput("Мед", 0.5, Money.FromRubles(450.00m), Money.FromRubles(225.00m), null),
                new ProductInput("Орехи грецкие", 0.3, Money.FromRubles(650.00m), Money.FromRubles(195.00m), null),
                new ProductInput("Яблоки", 1.5, Money.FromRubles(140.00m), Money.FromRubles(210.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 3, 19, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 3, 20, 0, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256804", "12404", "6789012404"),
            new UserId(UserConstants.DefaultUserId),
            "Лента",
            [
                new ProductInput("Стейк говяжий", 0.5, Money.FromRubles(850.00m), Money.FromRubles(425.00m), null),
                new ProductInput("Картофель", 1.5, Money.FromRubles(45.00m), Money.FromRubles(68.00m), null),
                new ProductInput("Помидоры", 0.6, Money.FromRubles(180.00m), Money.FromRubles(108.00m), null),
                new ProductInput("Зелень", 1, Money.FromRubles(120.00m), Money.FromRubles(120.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 20, 18, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 20, 18, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256805", "12405", "6789012405"),
            new UserId(UserConstants.DefaultUserId),
            "Семишагов",
            [
                new ProductInput("Сыр бри", 0.3, Money.FromRubles(1200.00m), Money.FromRubles(360.00m), null),
                new ProductInput("Виноград", 0.8, Money.FromRubles(350.00m), Money.FromRubles(280.00m), null),
                new ProductInput("Орехи миндаль", 0.2, Money.FromRubles(850.00m), Money.FromRubles(170.00m), null),
                new ProductInput("Шоколад темный", 2, Money.FromRubles(180.00m), Money.FromRubles(360.00m), null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2025, 1, 5, 17, 0, 0, DateTimeKind.Utc),
            new DateTime(2025, 1, 5, 17, 30, 0, DateTimeKind.Utc),
            ReceiptFiscalData.Create("9288000100256806", "12406", "6789012406"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Куриное филе", 0.5, Money.FromRubles(420.00m), Money.FromRubles(210.00m), null),
                new ProductInput("Гречка", 1, Money.FromRubles(85.00m), Money.FromRubles(85.00m), null),
                new ProductInput("Огурцы", 0.5, Money.FromRubles(150.00m), Money.FromRubles(75.00m), null),
                new ProductInput("Сметана 20%", 1, Money.FromRubles(95.00m), Money.FromRubles(95.00m), null),
            ]
        ),
    ];
}