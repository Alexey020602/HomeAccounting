using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

sealed partial class ReceiptSpending : Spending
{
    public ReceiptFiscalData FiscalData { get; private set; }
    private List<Product> products = [];
    public IReadOnlyList<Product> Products => products;
    public string PurchasePlace { get; private set; }
    public ReceiptProcessingStatus Status { get; private set; }
    public Money DeclaredSum { get; private set; }
    private List<ReceiptProcessingAttempt> attempts = [];
    public IReadOnlyList<ReceiptProcessingAttempt> Attempts => attempts;
    public DateTime? CompletedAt { get; private set; }
    public DateTime? NextRetryAt { get; private set; }
    public DateTime? LastAttemptAt { get; private set; }
    public string? LastErrorMessage { get; private set; }

    public override Money Sum => Products.Count > 0 ? Products.Sum(p => p.Sum) : DeclaredSum;
    public override string Description => Status switch
    {
        ReceiptProcessingStatus.Processing => "Чек обрабатывается",
        ReceiptProcessingStatus.Succeeded => PurchasePlace,
        ReceiptProcessingStatus.Failed => "Ошибка обработки чека",
        _ => throw new DomainException("Unknown status")
    };

    
    private ReceiptSpending()
    {
        FiscalData = ReceiptFiscalData.Empty();
        PurchasePlace = string.Empty;
        DeclaredSum = Money.Zero;
        Status = ReceiptProcessingStatus.Processing;
    }

    public ReceiptSpending(
        DateTime purchaseDate, 
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs
    ) : base(purchaseDate, addedDate, userId)
    {
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;
        Status = ReceiptProcessingStatus.Succeeded;

        var productsToAdd = productInputs
            .Select(product => new Product(product.Name, product.Quantity, product.Price, product.Sum, product.CategoryId));

        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }

        DeclaredSum = Products.Sum(p => p.Sum);
        CompletedAt = addedDate;
    }

    public ReceiptSpending(
        DateTime purchaseDate,
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        Money declaredSum
    ) : base(purchaseDate, addedDate, userId)
    {
        FiscalData = fiscalData;
        DeclaredSum = declaredSum;
        Status = ReceiptProcessingStatus.Processing;
        PurchasePlace = string.Empty;
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

    public void MarkProcessingSucceeded(
        DateTime completedAt,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs
    )
    {
        Status = ReceiptProcessingStatus.Succeeded;
        CompletedAt = completedAt;
        NextRetryAt = null;
        LastAttemptAt = completedAt;
        LastErrorMessage = null;
        PurchasePlace = purchasePlace;
        ReplaceProducts(productInputs);
        DeclaredSum = Products.Sum(p => p.Sum);
        attempts.Add(ReceiptProcessingAttempt.Success(completedAt));
    }

    public void MarkProcessingRetryableError(
        DateTime attemptedAt,
        DateTime nextRetryAt,
        string errorMessage,
        int errorCode)
    {
        Status = ReceiptProcessingStatus.Processing;
        NextRetryAt = nextRetryAt;
        RegisterFailedAttempt(attemptedAt, errorMessage, errorCode);
    }

    public void MarkProcessingFailed(DateTime completedAt, string errorMessage, int errorCode)
    {
        Status = ReceiptProcessingStatus.Failed;
        CompletedAt = completedAt;
        NextRetryAt = null;
        RegisterFailedAttempt(completedAt, errorMessage, errorCode);
    }

    public bool CanRetry(DateTime now, int maxRetries)
    {
        if (Status != ReceiptProcessingStatus.Processing)
        {
            return false;
        }

        var retryCount = attempts.Count(attempt => !attempt.IsSuccess);
        if (retryCount >= maxRetries)
        {
            return false;
        }

        return NextRetryAt is null || NextRetryAt <= now;
    }

    private void ReplaceProducts(IEnumerable<ProductInput> productInputs)
    {
        products.Clear();

        var productsToAdd = productInputs
            .Select(product => new Product(product.Name, product.Quantity, product.Price, product.Sum, product.CategoryId));

        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }
    }
    private Product GetProduct(ProductId productId) => products.FirstOrDefault(p => p.Id == productId) ?? throw new DomainException("Product not found");

    private void RegisterFailedAttempt(DateTime attemptedAt, string errorMessage, int errorCode)
    {
        LastAttemptAt = attemptedAt;
        LastErrorMessage = errorMessage;
        attempts.Add(ReceiptProcessingAttempt.Failure(attemptedAt, errorMessage, errorCode));
    }
}