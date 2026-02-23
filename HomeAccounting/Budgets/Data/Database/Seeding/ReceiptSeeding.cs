using HomeAccounting.Common.Model;
using HomeAccounting.Common.Model.ValueObjects;
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
        var fn = FiscalNumber.Create("9282440300123456");
        var fd = FiscalDocument.Create("12345");
        var fp = FiscalSign.Create("1234567890");
        var fiscalSum = Money.FromRubles(1250.50m);
        var purchaseDate = new DateTimeOffset(2024, 10, 15, 11, 55, 0, TimeSpan.Zero);
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
            fn,
            fd,
            fp,
            fiscalSum,
            purchaseDate,
            userId,
            "Магнит",
            products);

        // Второй чек для первого бюджета
        var addedDate2 = new DateTimeOffset(2024, 10, 16, 14, 30, 0, TimeSpan.Zero);
        var fn2 = FiscalNumber.Create("9282440300111111");
        var fd2 = FiscalDocument.Create("11111");
        var fp2 = FiscalSign.Create("1111111111");
        var fiscalSum2 = Money.FromRubles(2340.75m);
        var purchaseDate2 = new DateTimeOffset(2024, 10, 16, 14, 25, 0, TimeSpan.Zero);
        var products2 = new[]
        {
            new ProductInput("Мясо говядина", 1.5, Money.FromRubles(450.00m), Money.FromRubles(675.00m), null),
            new ProductInput("Картофель", 3, Money.FromRubles(35.00m), Money.FromRubles(105.00m), null),
            new ProductInput("Морковь", 1, Money.FromRubles(40.00m), Money.FromRubles(40.00m), null),
            new ProductInput("Лук репчатый", 0.5, Money.FromRubles(50.00m), Money.FromRubles(25.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-1112-759b-b91a-1b51403e8039")),
            budgetId,
            addedDate2,
            fn2,
            fd2,
            fp2,
            fiscalSum2,
            purchaseDate2,
            userId,
            "Ашан",
            products2);

        // Третий чек для первого бюджета
        var addedDate3 = new DateTimeOffset(2024, 10, 17, 9, 15, 0, TimeSpan.Zero);
        var fn3 = FiscalNumber.Create("9282440300222222");
        var fd3 = FiscalDocument.Create("22222");
        var fp3 = FiscalSign.Create("2222222222");
        var fiscalSum3 = Money.FromRubles(567.30m);
        var purchaseDate3 = new DateTimeOffset(2024, 10, 17, 9, 10, 0, TimeSpan.Zero);
        var products3 = new[]
        {
            new ProductInput("Яйца куриные", 1, Money.FromRubles(120.00m), Money.FromRubles(120.00m), null),
            new ProductInput("Масло сливочное", 1, Money.FromRubles(180.00m), Money.FromRubles(180.00m), null),
            new ProductInput("Сметана", 2, Money.FromRubles(85.00m), Money.FromRubles(170.00m), null),
            new ProductInput("Творог", 1, Money.FromRubles(97.30m), Money.FromRubles(97.30m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-1113-759b-b91a-1b51403e8039")),
            budgetId,
            addedDate3,
            fn3,
            fd3,
            fp3,
            fiscalSum3,
            purchaseDate3,
            userId,
            "Перекрёсток",
            products3);

        // Четвертый чек для первого бюджета
        var addedDate4 = new DateTimeOffset(2024, 10, 18, 18, 45, 0, TimeSpan.Zero);
        var fn4 = FiscalNumber.Create("9282440300333333");
        var fd4 = FiscalDocument.Create("33333");
        var fp4 = FiscalSign.Create("3333333333");
        var fiscalSum4 = Money.FromRubles(1890.00m);
        var purchaseDate4 = new DateTimeOffset(2024, 10, 18, 18, 40, 0, TimeSpan.Zero);
        var products4 = new[]
        {
            new ProductInput("Курица целая", 1, Money.FromRubles(350.00m), Money.FromRubles(350.00m), null),
            new ProductInput("Рис", 2, Money.FromRubles(120.00m), Money.FromRubles(240.00m), null),
            new ProductInput("Огурцы", 1, Money.FromRubles(150.00m), Money.FromRubles(150.00m), null),
            new ProductInput("Помидоры", 1.5, Money.FromRubles(200.00m), Money.FromRubles(300.00m), null),
            new ProductInput("Зелень", 1, Money.FromRubles(50.00m), Money.FromRubles(50.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-1114-759b-b91a-1b51403e8039")),
            budgetId,
            addedDate4,
            fn4,
            fd4,
            fp4,
            fiscalSum4,
            purchaseDate4,
            userId,
            "Лента",
            products4);

        // Пятый чек для первого бюджета
        var addedDate5 = new DateTimeOffset(2024, 10, 19, 12, 20, 0, TimeSpan.Zero);
        var fn5 = FiscalNumber.Create("9282440300444444");
        var fd5 = FiscalDocument.Create("44444");
        var fp5 = FiscalSign.Create("4444444444");
        var fiscalSum5 = Money.FromRubles(425.50m);
        var purchaseDate5 = new DateTimeOffset(2024, 10, 19, 12, 15, 0, TimeSpan.Zero);
        var products5 = new[]
        {
            new ProductInput("Батон нарезной", 1, Money.FromRubles(55.00m), Money.FromRubles(55.00m), null),
            new ProductInput("Колбаса докторская", 0.5, Money.FromRubles(450.00m), Money.FromRubles(225.00m), null),
            new ProductInput("Сыр плавленый", 1, Money.FromRubles(145.50m), Money.FromRubles(145.50m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-1115-759b-b91a-1b51403e8039")),
            budgetId,
            addedDate5,
            fn5,
            fd5,
            fp5,
            fiscalSum5,
            purchaseDate5,
            userId,
            "Магнит",
            products5);
    }

    public static IEnumerable<Receipt> GetDefaultReceiptsForSecondBudget()
    {
        var budgetId = new BudgetId(Budget.SecondBudgetId);
        var userId = new UserId(UserConstants.DefaultUserId);
        var addedDate = new DateTimeOffset(2024, 10, 20, 19, 0, 0, TimeSpan.Zero);
        var fn = FiscalNumber.Create("9282440300999999");
        var fd = FiscalDocument.Create("54321");
        var fp = FiscalSign.Create("9876543210");
        var fiscalSum = Money.FromRubles(890.00m);
        var purchaseDate = new DateTimeOffset(2024, 10, 20, 18, 50, 0, TimeSpan.Zero);
        var products = new[]
        {
            new ProductInput("Кофе зерновой", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Печенье", 1, Money.FromRubles(440.00m), Money.FromRubles(440.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2222-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate,
            fn,
            fd,
            fp,
            fiscalSum,
            purchaseDate,
            userId,
            "Пятёрочка",
            products);

        // Второй чек для второго бюджета
        var addedDate2 = new DateTimeOffset(2024, 10, 21, 16, 0, 0, TimeSpan.Zero);
        var fn2 = FiscalNumber.Create("9282440300555555");
        var fd2 = FiscalDocument.Create("55555");
        var fp2 = FiscalSign.Create("5555555555");
        var fiscalSum2 = Money.FromRubles(1234.00m);
        var purchaseDate2 = new DateTimeOffset(2024, 10, 21, 15, 55, 0, TimeSpan.Zero);
        var products2 = new[]
        {
            new ProductInput("Чай зелёный", 1, Money.FromRubles(280.00m), Money.FromRubles(280.00m), null),
            new ProductInput("Сахар", 2, Money.FromRubles(65.00m), Money.FromRubles(130.00m), null),
            new ProductInput("Мёд", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Лимон", 0.5, Money.FromRubles(120.00m), Money.FromRubles(60.00m), null),
            new ProductInput("Имбирь", 0.1, Money.FromRubles(214.00m), Money.FromRubles(21.40m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2223-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate2,
            fn2,
            fd2,
            fp2,
            fiscalSum2,
            purchaseDate2,
            userId,
            "Азбука Вкуса",
            products2);

        // Третий чек для второго бюджета
        var addedDate3 = new DateTimeOffset(2024, 10, 22, 10, 30, 0, TimeSpan.Zero);
        var fn3 = FiscalNumber.Create("9282440300666666");
        var fd3 = FiscalDocument.Create("66666");
        var fp3 = FiscalSign.Create("6666666666");
        var fiscalSum3 = Money.FromRubles(2780.00m);
        var purchaseDate3 = new DateTimeOffset(2024, 10, 22, 10, 25, 0, TimeSpan.Zero);
        var products3 = new[]
        {
            new ProductInput("Лосось", 0.8, Money.FromRubles(1200.00m), Money.FromRubles(960.00m), null),
            new ProductInput("Авокадо", 2, Money.FromRubles(180.00m), Money.FromRubles(360.00m), null),
            new ProductInput("Сыр пармезан", 0.3, Money.FromRubles(1500.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Оливковое масло", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Руккола", 1, Money.FromRubles(200.00m), Money.FromRubles(200.00m), null),
            new ProductInput("Лимон", 1, Money.FromRubles(120.00m), Money.FromRubles(120.00m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2224-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate3,
            fn3,
            fd3,
            fp3,
            fiscalSum3,
            purchaseDate3,
            userId,
            "ВкусВилл",
            products3);

        // Четвертый чек для второго бюджета
        var addedDate4 = new DateTimeOffset(2024, 10, 23, 19, 15, 0, TimeSpan.Zero);
        var fn4 = FiscalNumber.Create("9282440300777777");
        var fd4 = FiscalDocument.Create("77777");
        var fp4 = FiscalSign.Create("7777777777");
        var fiscalSum4 = Money.FromRubles(1567.80m);
        var purchaseDate4 = new DateTimeOffset(2024, 10, 23, 19, 10, 0, TimeSpan.Zero);
        var products4 = new[]
        {
            new ProductInput("Вино красное", 1, Money.FromRubles(850.00m), Money.FromRubles(850.00m), null),
            new ProductInput("Сыр бри", 1, Money.FromRubles(450.00m), Money.FromRubles(450.00m), null),
            new ProductInput("Виноград", 1, Money.FromRubles(180.00m), Money.FromRubles(180.00m), null),
            new ProductInput("Орехи грецкие", 0.3, Money.FromRubles(292.67m), Money.FromRubles(87.80m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2225-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate4,
            fn4,
            fd4,
            fp4,
            fiscalSum4,
            purchaseDate4,
            userId,
            "Азбука Вкуса",
            products4);

        // Пятый чек для второго бюджета
        var addedDate5 = new DateTimeOffset(2024, 10, 24, 13, 45, 0, TimeSpan.Zero);
        var fn5 = FiscalNumber.Create("9282440300888888");
        var fd5 = FiscalDocument.Create("88888");
        var fp5 = FiscalSign.Create("8888888888");
        var fiscalSum5 = Money.FromRubles(2345.60m);
        var purchaseDate5 = new DateTimeOffset(2024, 10, 24, 13, 40, 0, TimeSpan.Zero);
        var products5 = new[]
        {
            new ProductInput("Говядина вырезка", 1.2, Money.FromRubles(650.00m), Money.FromRubles(780.00m), null),
            new ProductInput("Грибы шампиньоны", 0.5, Money.FromRubles(280.00m), Money.FromRubles(140.00m), null),
            new ProductInput("Лук красный", 0.3, Money.FromRubles(80.00m), Money.FromRubles(24.00m), null),
            new ProductInput("Перец болгарский", 1, Money.FromRubles(150.00m), Money.FromRubles(150.00m), null),
            new ProductInput("Чеснок", 0.1, Money.FromRubles(200.00m), Money.FromRubles(20.00m), null),
            new ProductInput("Вино белое", 1, Money.FromRubles(750.00m), Money.FromRubles(750.00m), null),
            new ProductInput("Сливки 33%", 1, Money.FromRubles(280.00m), Money.FromRubles(280.00m), null),
            new ProductInput("Масло сливочное", 1, Money.FromRubles(201.60m), Money.FromRubles(201.60m), null),
        };
        yield return new Receipt(
            new ReceiptId(Guid.Parse("01989e7d-2226-75e8-91a1-7edc1f9b3385")),
            budgetId,
            addedDate5,
            fn5,
            fd5,
            fp5,
            fiscalSum5,
            purchaseDate5,
            userId,
            "ВкусВилл",
            products5);
    }
}
