using Microsoft.EntityFrameworkCore;
using Receipts.DataBase.Entities;

namespace Receipts.DataBase;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SetupChecksForDevelopment(this DbContextOptionsBuilder options) =>
        options
            .UseSeeding(Seed)
            .UseAsyncSeeding(SeedAsync);

    private static void Seed(DbContext context, bool dbHasChanged)
    {
        if (context is not ReceiptsContext checksContext) return;

        checksContext.ExistingCategories().Load();
        checksContext.AddChecks();
        checksContext.SaveChanges();
    }

    private static async Task SeedAsync(DbContext context, bool dbHasChanged, CancellationToken cancellationToken)
    {
        if (context is not ReceiptsContext checksContext) return;

        await checksContext.ExistingCategories().LoadAsync(cancellationToken);
        await checksContext.AddChecksAsync(cancellationToken);
        await checksContext.SaveChangesAsync(cancellationToken);
    }

    private static void AddChecks(this ReceiptsContext context)
    {
        foreach (var check in Checks)
        {
            if (context.Checks.SingleOrDefault(c => c.PurchaseDate == check.PurchaseDate && c.Fn == check.Fn && c.Fd == check.Fd && c.Fp == check.Fp && c.S == check.S) is not null) continue;
            
            context.Checks.Add(context.CreateCheck(check));
        }
    }

    private static async Task AddChecksAsync(this ReceiptsContext context, CancellationToken cancellationToken)
    {
        foreach (var check in Checks)
        {
            if (await context.Checks.SingleOrDefaultAsync(c => c.PurchaseDate == check.PurchaseDate && c.Fn == check.Fn && c.Fd == check.Fd && c.Fp == check.Fp && c.S == check.S, cancellationToken) is not null) continue;

            context.Checks.Add(context.CreateCheck(check));
        }
    }

    private static Entities.Check CreateCheck(this ReceiptsContext context, Check check) => new Entities.Check()
    {
        PurchaseDate = check.PurchaseDate,
        AddedDate = check.AddedDate,
        Fd = check.Fd,
        Fn = check.Fn,
        Fp = check.Fp,
        S = check.S,
        Products = check.Products.Select(context.CreateProduct).ToList(),
        Login = "chillexey",
    };

    private static Entities.Product CreateProduct(this ReceiptsContext context, Product product)
    {
        return new Entities.Product()
        {
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price,
            Sum = product.Sum,
            Subcategory = context.GetSubcategory(product.Subcategory, product.Category),
        };
    }

    private static Category GetCategory(this ReceiptsContext context, string categoryName)
    {
        if (context.GetExistingCategory(categoryName) is { } category) return category;

        var newCategory = new Category()
        {
            Name = categoryName,
        };

        context.Categories.Add(newCategory);

        return newCategory;
    }

    private static Subcategory GetSubcategory(
        this ReceiptsContext context, 
        string? subcategoryName, 
        string categoryName
        )
    {
        var category = context.GetCategory(categoryName);
        // if (context.Subcategories.Local.SingleOrDefault(c => c.Name == categoryName && c.CategoryId == category.Id) is
            // { } subcategory) return subcategory;

        if(category.Subcategories.SingleOrDefault(s => s.Name == subcategoryName) is {} subcategory) return subcategory;
        
        var newSubcategory = new Subcategory()
        {
            Name = subcategoryName,
            Category = category,
        };
        category.Subcategories.Add(newSubcategory);
        
        return newSubcategory;
    }

    private static Category? GetExistingCategory(this ReceiptsContext context, string categoryName) =>
        context.Categories.Local.SingleOrDefault(c => c.Name == categoryName);

    private static IQueryable<Category> ExistingCategories(this ReceiptsContext context) => context.Categories
        .Where(category =>
            CategoriesNames
                .Any(categoryName => categoryName == category.Name)
        )
        .Include(category => category.Subcategories
            .Where(subcategory =>
                SubcategoriesNames
                    .Any(subcategoryName => subcategoryName == subcategory.Name)
            )
        );

    // private static IQueryable<Entities.Check> ExistingChecks(this ChecksContext context) =>
    //     context.Receipts
    //         .Where(check => ChecksData.Any(data => check.PurchaseDate == data.Item1 && check.Fd == data.Item2 && check.Fn == data.Item3 && check.Fp == data.Item4 && check.S == data.Item5));
    // private static IQueryable<Subcategory> ExistingSubcategories(this ChecksContext checksContext) => checksContext
    //     .Subcategories
    //     .Where(subcategory =>
    //         SubcategoriesNames
    //             .Any(subcategoryName => subcategoryName == subcategory.Name)
    //     );
    private static IEnumerable<string> CategoriesNames =>
        Checks.SelectMany(check => check.Products.Select(product => product.Category));

    private static IEnumerable<string?> SubcategoriesNames =>
        Checks.SelectMany(check => check.Products.Select(product => product.Subcategory));
    
    private static readonly IReadOnlyList<Check> Checks =
    [
        new
        (
            new DateTime(2025, 01, 07, 10, 41, 57, DateTimeKind.Utc),
            DateTime.UtcNow,
            "9446",
            "7380440801290534",
            "1880975916",
            "1691.16",
            [
                new("Филе грудок цыпленка-бройлера охл. 1кг", "Мясная гастрономия", "Продукты", 35999, 44711,
                    1.242),
                new("Молоко \"Ясь Белоус\" у/паст. 3,3-6% 930мл БЗМЖ", "Молоко и молочные продукты", "Продукты",
                    7699, 7699, 1),
                new("Сливки \"Большая Кружка\" 20% 500г БЗМЖ", "Молоко и молочные продукты", "Продукты", 16999,
                    16999, 1),
                new("Средство д/сантех. \"Санокс ультра\" с ускорителем чистки 750г", "Бытовая химия",
                    "Товары для дома", 9999, 9999, 1),
                new("Средство чист. \"Белизна-Гель\" универс. 1л", "Бытовая химия", "Товары для дома", 5499, 5499,
                    1),
                new("Сыр \"Голландский\" 45% 1кг БЗМЖ, Беларусь", "Сырная продукция", "Продукты", 69999, 46759,
                    0.668),
                new("Сахарный песок 1кг", "Соль, специи, сахар", "Продукты", 4999, 4999, 1),
                new("Изделия макаронные \"Знатные\" рожки витые гр.А в/с 400г",
                    "Крупы, бобовые, макаронные изделия", "Продукты", 3999, 7998, 2),
                new("Изделия макаронные \"Знатные\" спиральки гр.А в/с 400г", "Крупы, бобовые, макаронные изделия",
                    "Продукты", 3999, 7998, 2),
                new("Паста зубная \"Аквафреш\" Освежающе-мятная 125мл", "Средства и предметы гигиены",
                    "Товары для красоты", 12999, 12999, 1),
                new("Лук репчатый 1кг", "Фрукты, овощи и грибы", "Продукты", 3999, 1056, 0.264),
                new("Чеснок 1кг", "Фрукты, овощи и грибы", "Продукты", 39999, 2400, 0.06),
            ]
        ),
        new
        (
            new DateTime(2025, 01, 12, 17, 15, 00, DateTimeKind.Utc),
            DateTime.UtcNow,
            "21728",
            "7381440800435707",
            "3846028993",
            "93.00",
            [
                new("Кисть 50х10мм нат.щетин Zolder Standart", "Инструменты", "Строительство и ремонт", 5300, 530,
                    1),
                new("Кисть 38х10мм нат.щетин Zolder Standart", "Инструменты", "Строительство и ремонт", 4000, 4000,
                    1),
            ]
        ),
        new
        (
            new DateTime(2025, 01, 13, 23, 21, 00, DateTimeKind.Utc),
            DateTime.UtcNow,
            "62887",
            "7284440500173838",
            "182171056",
            "1015.73",
            [
                new("ЛЕН.Мор.сл.ар.к.кеш.сах450г", "Мороженое", "Продукты", 3999, 3999, 1),
                new("ПИСК.Мол.паст.2,5% 1л", "Молоко и молочные продукты", "Продукты", 8199, 8199, 1),
                new("Р.ВК.Сыр ТОПЛЕН.МОЛОЧКО 45% 1кг", "Сырная продукция", "Продукты", 134990, 52376, 0.388),
                new("ПЕРЕКРЕСТОК Пакет майка", "Хозяйственные товары", "Товары для дома", 999, 999, 1),
            ]
        ),
        new
        (
            new DateTime(2025, 01, 11, 10, 59, 07, DateTimeKind.Utc),
            DateTime.UtcNow,
            "10277",
            "7380440801290534",
            "840967215",
            "1236.35",
            [
                new("Полотенца универ. \"Эконом смарт\" 150шт", "Хозяйственные товары", "Товары для дома", 17999,
                    17999, 1),
                new("Полотенца бум. \"Мягкий знак\" Делюкс 2сл 2рул", "Хозяйственные товары", "Товары для дома",
                    9999, 9999, 1),
                new("Яйцо С0 1дес", "Яйца", "Продукты", 10799, 10799, 1),
                new("Прованские травы(9 трав) \"Приправыч\" 30г", "Соль, специи, сахар", "Продукты", 9999, 9999, 1),
                new("Пюре томатное \"Краснодарская паста\" ТУ ст/б 270г", "Детское питание", "Продукты", 4999, 4999,
                    1),
                new("Томаты 1кг", "Фрукты, овощи и грибы", "Продукты", 17999, 7946, 0.442),
                new("Лук репчатый 1кг", "Фрукты, овощи и грибы", "Продукты", 3999, 800, 0.2),
                new("Щетка зубная компл. \"Сплат\" Очищение и уход средняя в асс. 1шт",
                    "Средства и предметы гигиены", "Товары для красоты", 9999, 9999, 1),
                new("Хлебец \"Наливной\" бездрож. формовой нарез. 300г, Каравай", "Хлеб и выпечка", "Продукты",
                    4999, 4999, 1),
                new("Фарш Домашний охл. 1кг", "Мясная гастрономия", "Продукты", 36999, 23087, 0.624),
                new("Пирожное \"Бисквитное глаз. с кремом\" 4*70г, Невские берега",
                    "Торты, пирожные, суфле, десерты", "Продукты", 22999, 22999, 1),
            ]
        ),
        new
        (
            new DateTime(2025, 01, 04, 12, 33, 00, DateTimeKind.Utc),
            DateTime.UtcNow,
            "26636",
            "7380440700673345",
            "1241320200",
            "1289.00",
            [
                new("*Вино ГАН.АСТИ D.O.C.G и.0.75л", "Вино", "Алкоголь", 128900, 128900, 1),
            ]
        )
    ];

    record Check(
        DateTime PurchaseDate,
        DateTime AddedDate,
        string Fd,
        string Fn,
        string Fp,
        string S,
        IReadOnlyList<Product> Products);

    record Product(string Name, string? Subcategory, string Category, int Price, int Sum, double Quantity);
}