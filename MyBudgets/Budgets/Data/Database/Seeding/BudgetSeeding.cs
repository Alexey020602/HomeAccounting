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
            15000,
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
            3500,
            new DateTime(2024, 10, 5, 14, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 5, 15, 0, 0, DateTimeKind.Utc),
            "Оплата интернета",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            2800,
            new DateTime(2024, 10, 12, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 12, 10, 15, 0, DateTimeKind.Utc),
            "Оплата мобильной связи",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            12000,
            new DateTime(2024, 10, 20, 18, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 20, 18, 30, 0, DateTimeKind.Utc),
            "Поход в кино",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            4500,
            new DateTime(2024, 11, 3, 12, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 3, 12, 20, 0, DateTimeKind.Utc),
            "Обед в ресторане",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            8500,
            new DateTime(2024, 11, 15, 16, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 15, 16, 45, 0, DateTimeKind.Utc),
            "Подарок на день рождения",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            2200,
            new DateTime(2024, 11, 25, 9, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 25, 9, 10, 0, DateTimeKind.Utc),
            "Такси",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            15000,
            new DateTime(2024, 12, 10, 19, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 10, 19, 30, 0, DateTimeKind.Utc),
            "Новогодние подарки",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            3200,
            new DateTime(2024, 12, 20, 11, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 20, 11, 15, 0, DateTimeKind.Utc),
            "Оплата подписки на стриминг",
            new UserId(UserConstants.SecondUserId)
        ),
        new ManualSpending(
            6800,
            new DateTime(2025, 1, 5, 13, 0, 0, DateTimeKind.Utc),
            new DateTime(2025, 1, 5, 13, 20, 0, DateTimeKind.Utc),
            "Обед в кафе",
            new UserId(UserConstants.DefaultUserId)
        ),
    ];

    public static IEnumerable<ManualSpending> GetDefaultManualSpendingsForSecondBudget() =>
    [
        new ManualSpending(
            2500,
            new DateTime(2024, 10, 8, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 8, 8, 10, 0, DateTimeKind.Utc),
            "Оплата проездного",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            1800,
            new DateTime(2024, 10, 15, 20, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 15, 20, 15, 0, DateTimeKind.Utc),
            "Кофе и завтрак",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            4200,
            new DateTime(2024, 11, 1, 17, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 1, 17, 30, 0, DateTimeKind.Utc),
            "Книги",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            9500,
            new DateTime(2024, 11, 10, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 10, 15, 45, 0, DateTimeKind.Utc),
            "Одежда",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            1200,
            new DateTime(2024, 11, 22, 12, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 22, 12, 10, 0, DateTimeKind.Utc),
            "Обед",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            3500,
            new DateTime(2024, 12, 5, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 5, 10, 20, 0, DateTimeKind.Utc),
            "Фитнес-абонемент",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            2800,
            new DateTime(2024, 12, 18, 14, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 18, 14, 15, 0, DateTimeKind.Utc),
            "Косметика",
            new UserId(UserConstants.DefaultUserId)
        ),
        new ManualSpending(
            1500,
            new DateTime(2025, 1, 3, 9, 0, 0, DateTimeKind.Utc),
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
            new ReceiptFiscalData("9288000100256789", "12345", "6789012345"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Хлеб белый", 1, 65, 65, null),
                new ProductInput("Молоко 3.2%", 1, 89, 89, null),
                new ProductInput("Яйца куриные С0", 1, 125, 125, null),
                new ProductInput("Сыр Российский", 0.5, 450, 225, null),
                new ProductInput("Помидоры", 0.8, 180, 144, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 14, 19, 15, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 14, 19, 45, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256790", "12346", "6789012346"),
            new UserId(UserConstants.SecondUserId),
            "Магнит",
            [
                new ProductInput("Курица охлажденная", 1.2, 350, 420, null),
                new ProductInput("Картофель", 2.0, 45, 90, null),
                new ProductInput("Лук репчатый", 0.5, 60, 30, null),
                new ProductInput("Морковь", 0.6, 55, 33, null),
                new ProductInput("Масло подсолнечное", 1, 185, 185, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 21, 17, 45, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 21, 18, 15, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256791", "12347", "6789012347"),
            new UserId(UserConstants.DefaultUserId),
            "Перекресток",
            [
                new ProductInput("Говядина вырезка", 0.8, 650, 520, null),
                new ProductInput("Сметана 20%", 1, 95, 95, null),
                new ProductInput("Творог 5%", 0.4, 180, 72, null),
                new ProductInput("Бананы", 1.5, 120, 180, null),
                new ProductInput("Яблоки", 1.0, 140, 140, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 5, 16, 20, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 5, 16, 50, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256792", "12348", "6789012348"),
            new UserId(UserConstants.SecondUserId),
            "Лента",
            [
                new ProductInput("Рыба семга", 0.5, 1200, 600, null),
                new ProductInput("Рис", 1, 95, 95, null),
                new ProductInput("Огурцы", 0.7, 150, 105, null),
                new ProductInput("Салат листовой", 1, 120, 120, null),
                new ProductInput("Оливковое масло", 1, 450, 450, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 18, 20, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 18, 20, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256793", "12349", "6789012349"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Колбаса докторская", 0.3, 450, 135, null),
                new ProductInput("Хлеб ржаной", 1, 55, 55, null),
                new ProductInput("Масло сливочное", 0.2, 550, 110, null),
                new ProductInput("Чай черный", 1, 180, 180, null),
                new ProductInput("Кофе молотый", 1, 320, 320, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 8, 15, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 8, 16, 0, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256794", "12350", "6789012350"),
            new UserId(UserConstants.SecondUserId),
            "Семишагов",
            [
                new ProductInput("Шампанское", 1, 450, 450, null),
                new ProductInput("Сыр пармезан", 0.2, 1200, 240, null),
                new ProductInput("Ветчина", 0.3, 550, 165, null),
                new ProductInput("Икра красная", 1, 850, 850, null),
                new ProductInput("Хлеб белый нарезка", 1, 75, 75, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 22, 18, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 22, 18, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256795", "12351", "6789012351"),
            new UserId(UserConstants.DefaultUserId),
            "Ашан",
            [
                new ProductInput("Мандарины", 2.0, 120, 240, null),
                new ProductInput("Шоколад", 3, 95, 285, null),
                new ProductInput("Печенье", 2, 85, 170, null),
                new ProductInput("Сок апельсиновый", 2, 120, 240, null),
                new ProductInput("Йогурт", 4, 65, 260, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2025, 1, 8, 17, 15, 0, DateTimeKind.Utc),
            new DateTime(2025, 1, 8, 17, 45, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256796", "12352", "6789012352"),
            new UserId(UserConstants.SecondUserId),
            "Пятерочка",
            [
                new ProductInput("Курица целая", 1.5, 280, 420, null),
                new ProductInput("Гречка", 1, 85, 85, null),
                new ProductInput("Лук зеленый", 1, 90, 90, null),
                new ProductInput("Укроп", 1, 75, 75, null),
                new ProductInput("Сметана 15%", 1, 85, 85, null),
            ]
        ),
    ];

    public static IEnumerable<ReceiptSpending> GetDefaultReceiptSpendingsForSecondBudget() =>
    [
        new ReceiptSpending(
            new DateTime(2024, 10, 10, 19, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 10, 19, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256800", "12400", "6789012400"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Хлеб белый", 1, 65, 65, null),
                new ProductInput("Молоко 2.5%", 1, 79, 79, null),
                new ProductInput("Йогурт", 2, 65, 130, null),
                new ProductInput("Банан", 1.2, 120, 144, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 10, 18, 18, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 10, 18, 19, 0, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256801", "12401", "6789012401"),
            new UserId(UserConstants.DefaultUserId),
            "Магнит",
            [
                new ProductInput("Куриная грудка", 0.6, 380, 228, null),
                new ProductInput("Рис", 1, 95, 95, null),
                new ProductInput("Брокколи", 0.4, 200, 80, null),
                new ProductInput("Морковь", 0.5, 55, 28, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 5, 17, 45, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 5, 18, 15, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256802", "12402", "6789012402"),
            new UserId(UserConstants.DefaultUserId),
            "Перекресток",
            [
                new ProductInput("Лосось", 0.4, 950, 380, null),
                new ProductInput("Авокадо", 2, 180, 360, null),
                new ProductInput("Салат айсберг", 1, 150, 150, null),
                new ProductInput("Оливковое масло", 1, 450, 450, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 11, 15, 16, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 11, 15, 16, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256803", "12403", "6789012403"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Творог 9%", 0.4, 200, 80, null),
                new ProductInput("Мед", 0.5, 450, 225, null),
                new ProductInput("Орехи грецкие", 0.3, 650, 195, null),
                new ProductInput("Яблоки", 1.5, 140, 210, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 3, 19, 30, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 3, 20, 0, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256804", "12404", "6789012404"),
            new UserId(UserConstants.DefaultUserId),
            "Лента",
            [
                new ProductInput("Стейк говяжий", 0.5, 850, 425, null),
                new ProductInput("Картофель", 1.5, 45, 68, null),
                new ProductInput("Помидоры", 0.6, 180, 108, null),
                new ProductInput("Зелень", 1, 120, 120, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2024, 12, 20, 18, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 12, 20, 18, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256805", "12405", "6789012405"),
            new UserId(UserConstants.DefaultUserId),
            "Семишагов",
            [
                new ProductInput("Сыр бри", 0.3, 1200, 360, null),
                new ProductInput("Виноград", 0.8, 350, 280, null),
                new ProductInput("Орехи миндаль", 0.2, 850, 170, null),
                new ProductInput("Шоколад темный", 2, 180, 360, null),
            ]
        ),
        new ReceiptSpending(
            new DateTime(2025, 1, 5, 17, 0, 0, DateTimeKind.Utc),
            new DateTime(2025, 1, 5, 17, 30, 0, DateTimeKind.Utc),
            new ReceiptFiscalData("9288000100256806", "12406", "6789012406"),
            new UserId(UserConstants.DefaultUserId),
            "Пятерочка",
            [
                new ProductInput("Куриное филе", 0.5, 420, 210, null),
                new ProductInput("Гречка", 1, 85, 85, null),
                new ProductInput("Огурцы", 0.5, 150, 75, null),
                new ProductInput("Сметана 20%", 1, 95, 95, null),
            ]
        ),
    ];
}