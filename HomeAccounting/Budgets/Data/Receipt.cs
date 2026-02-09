using ClientServerShared.Model.Money;
using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

internal sealed partial class Receipt : Entity<ReceiptId>
{
    public BudgetId BudgetId { get; private set; }
    public UserId UserId { get; private set; }
    public ReceiptFiscalData FiscalData { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string PurchasePlace { get; private set; } = string.Empty;
    public ReceiptProcessingStatus Status { get; private set; }
    private List<Product> products = [];
    public IReadOnlyList<Product> Products => products;
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? LastErrorMessage { get; private set; }

    public Money Sum => Products.Count > 0 ? Products.Sum(p => p.Sum) : FiscalData.Sum;

    public string Description => Status switch
    {
        ReceiptProcessingStatus.Processing => "Чек обрабатывается",
        ReceiptProcessingStatus.Succeeded => PurchasePlace,
        ReceiptProcessingStatus.Failed => "Ошибка обработки чека",
        _ => throw new DomainException("Unknown status")
    };

    private Receipt()
    {
        FiscalData = ReceiptFiscalData.Empty();
    }

    public Receipt(
        ReceiptId id,
        BudgetId budgetId,
        DateTimeOffset createdAt,
        ReceiptFiscalData fiscalData,
        UserId userId)
        : base(id)
    {
        BudgetId = budgetId;
        UserId = userId;
        FiscalData = fiscalData;
        CreatedAt = createdAt;
        PurchasePlace = string.Empty;
        Status = ReceiptProcessingStatus.Processing;
    }

    public Receipt(
        BudgetId budgetId,
        DateTimeOffset createdAt,
        ReceiptFiscalData fiscalData,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
    {
        BudgetId = budgetId;
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;
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
        ReceiptFiscalData fiscalData,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
        : base(id)
    {
        BudgetId = budgetId;
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;
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
}
