using System.Diagnostics.CodeAnalysis;
using ClientServerShared.Model.Money;
using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

internal sealed partial class Receipt : Entity<ReceiptId>
{
    public BudgetId BudgetId { get; private set; }
    public UserId UserId { get; private set; }
    
    // Фискальные данные (Value Objects)
    public FiscalNumber Fn { get; private set; }
    public FiscalDocument Fd { get; private set; }
    public FiscalSign Fp { get; private set; }
    
    // Фискальная сумма из чека
    public Money Sum { get; private set; }
    
    // Дата покупки
    public DateTimeOffset PurchaseDate { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    public string PurchasePlace { get; private set; } = string.Empty;
    public ReceiptProcessingStatus Status { get; private set; }
    private List<Product> products = [];
    public IReadOnlyList<Product> Products => products;
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? LastErrorMessage { get; private set; }

    /// <summary>
    /// Вычисляемая сумма из продуктов (если есть).
    /// </summary>
    public Money? CalculatedSum => Products.Count > 0 ? Products.Sum(p => p.Sum) : null;

    public string Description => Status switch
    {
        ReceiptProcessingStatus.Processing => "Чек обрабатывается",
        ReceiptProcessingStatus.Succeeded => PurchasePlace,
        ReceiptProcessingStatus.Failed => "Ошибка обработки чека",
        _ => throw new DomainException("Unknown status")
    };

    private Receipt()
    {
        // EF Core parameterless constructor
        Fn = FiscalNumber.Create("0000000000000000");
        Fd = FiscalDocument.Create("000");
        Fp = FiscalSign.Create("00000000");
        Sum = default;
        PurchaseDate = default;
    }

    public Receipt(
        ReceiptId id,
        BudgetId budgetId,
        DateTimeOffset createdAt,
        FiscalNumber fn,
        FiscalDocument fd,
        FiscalSign fp,
        Money sum,
        DateTimeOffset purchaseDate,
        UserId userId)
        : base(id)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future", nameof(purchaseDate));
        }
        
        BudgetId = budgetId;
        UserId = userId;
        Fn = fn;
        Fd = fd;
        Fp = fp;
        Sum = sum;
        PurchaseDate = purchaseDate;
        CreatedAt = createdAt;
        PurchasePlace = string.Empty;
        Status = ReceiptProcessingStatus.Processing;
    }

    public Receipt(
        BudgetId budgetId,
        DateTimeOffset createdAt,
        FiscalNumber fn,
        FiscalDocument fd,
        FiscalSign fp,
        Money sum,
        DateTimeOffset purchaseDate,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future", nameof(purchaseDate));
        }
        
        BudgetId = budgetId;
        PurchasePlace = purchasePlace;
        Fn = fn;
        Fd = fd;
        Fp = fp;
        Sum = sum;
        PurchaseDate = purchaseDate;
        Status = ReceiptProcessingStatus.Succeeded;
        UserId = userId;
        CreatedAt = createdAt;
        CompletedAt = createdAt;

        var productsToAdd = productInputs
            .Select(p => new Product(p.Name, p.Quantity, p.Price, p.Sum, p.CategoryId));
        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }
    }

    /// <summary>
    /// Constructor for already-processed receipt with explicit id (e.g. seeding).
    /// </summary>
    public Receipt(
        ReceiptId id,
        BudgetId budgetId,
        DateTimeOffset createdAt,
        FiscalNumber fn,
        FiscalDocument fd,
        FiscalSign fp,
        Money sum,
        DateTimeOffset purchaseDate,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
        : base(id)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future", nameof(purchaseDate));
        }
        
        BudgetId = budgetId;
        PurchasePlace = purchasePlace;
        Fn = fn;
        Fd = fd;
        Fp = fp;
        Sum = sum;
        PurchaseDate = purchaseDate;
        Status = ReceiptProcessingStatus.Succeeded;
        UserId = userId;
        CreatedAt = createdAt;
        CompletedAt = createdAt;

        var productsToAdd = productInputs
            .Select(p => new Product(p.Name, p.Quantity, p.Price, p.Sum, p.CategoryId));
        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }
    }

    public void MarkProcessingSucceeded(
        DateTimeOffset completedAt,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
    {
        Status = ReceiptProcessingStatus.Succeeded;
        CompletedAt = completedAt;
        LastErrorMessage = null;
        PurchasePlace = purchasePlace;
        ReplaceProducts(productInputs);
    }

    public void MarkProcessingFailed(DateTimeOffset completedAt, string errorMessage)
    {
        Status = ReceiptProcessingStatus.Failed;
        CompletedAt = completedAt;
        LastErrorMessage = errorMessage;
    }

    public void ChangeCategoryForProduct(ProductId productId, CategoryId categoryId)
    {
        var product = GetProduct(productId);
        product.ChangeCategory(categoryId);
    }

    public void DeleteCategoryForProduct(ProductId productId)
    {
        var product = GetProduct(productId);
        product.DeleteCategory();
    }

    private void ReplaceProducts(IEnumerable<ProductInput> productInputs)
    {
        products.Clear();
        var productsToAdd = productInputs
            .Select(p => new Product(p.Name, p.Quantity, p.Price, p.Sum, p.CategoryId));
        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }
    }

    private Product GetProduct(ProductId productId) =>
        products.FirstOrDefault(p => p.Id == productId) ?? throw new DomainException("Product not found");

    /// <summary>
    /// Создает ReceiptFiscalData из полей Receipt для межмодульного взаимодействия.
    /// </summary>
    public ReceiptFiscalData ToReceiptFiscalData()
    {
        return ReceiptFiscalData.Create(Fn, Fd, Fp, Sum, PurchaseDate);
    }

    /// <summary>
    /// Формирует строку фискальных данных для внешних API в формате: fn={Fn}&i={Fd}&fp={Fp}&t={PurchaseDate}&s={FiscalSum}&n=1
    /// </summary>
    public string GetRawFiscalDataString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = "yyyyMMddTHHmm")
    {
        return $"fn={Fn}&i={Fd}&fp={Fp}&t={PurchaseDate.ToString(format)}&s={Sum}&n=1";
    }
}
