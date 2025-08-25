using Microsoft.EntityFrameworkCore;
using Receipts.Core;
using Receipts.Core.GetReceipts;
using Receipts.Core.ReceiptCategorization;
using Receipts.DataBase.Entities;

namespace Receipts.DataBase;

public sealed class ReceiptSaveService(ReceiptsContext context) : IReceiptSaveService
{
    public async Task SaveReceipt(AddCheckRequest addCheckRequest)
    {
        var categoriesNames = addCheckRequest.Products.Select(p => p.Category);
        var subcategoriesNames = addCheckRequest.Products.Select(p => p.Subcategory);
        await context.Categories
            .Where(category => categoriesNames.Any(productCategory => productCategory == category.Name))
            .LoadAsync();
        await context.Subcategories
            .Where(subcategory => subcategoriesNames.Any(productSubcategory => productSubcategory == subcategory.Name))
            .LoadAsync();

        var check = new Check
        {
            Fp = addCheckRequest.ReceiptData.FiscalData.Fp,
            Fn = addCheckRequest.ReceiptData.FiscalData.Fn,
            Fd = addCheckRequest.ReceiptData.FiscalData.Fd,
            S = addCheckRequest.ReceiptData.FiscalData.S,
            AddedDate = DateTime.UtcNow,
            PurchaseDate = addCheckRequest.ReceiptData.FiscalData.T,
            Products = addCheckRequest.Products.Select(CreateProduct).ToList(),
            UserId = addCheckRequest.ReceiptData.UserId,
            BudgetId = addCheckRequest.ReceiptData.BudgetId
        };

        context.Checks.Add(check);
        await context.SaveChangesAsync();
    }
    
    private Product CreateProduct(AddCheckRequest.Product product)
    {
        var subcategory = GetSubcategoryByName(product.Subcategory, product.Category);
        return new Product
        {
            Name = product.Name,
            Price = product.Price,
            Sum = product.Sum,
            Quantity = product.Quantity,
            Subcategory = subcategory
        };
    }

    private Category CreateCategory(string name)
    {
        var category = new Category
        {
            Name = name
        };

        context.Categories.Add(category);

        return category;
    }

    private Subcategory CreateSubcategory(string? name, Category category)
    {
        var subcategory = new Subcategory
        {
            Name = name,
            Category = category
        };

        context.Subcategories.Add(subcategory);

        return subcategory;
    }

    private Subcategory GetSubcategoryByName(string? name, string categoryName)
    {
        if (GetExistingCategory(categoryName) is { } existingCategory)
            return GetExistingSubcategory(name, existingCategory) ??
                   CreateSubcategory(name, existingCategory);

        var category = CreateCategory(categoryName);

        return CreateSubcategory(name, category);
    }

    private Subcategory? GetExistingSubcategory(string? name, Category existingCategory) =>
        context.Subcategories.Local.SingleOrDefault(
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id));

    private static Func<Subcategory, bool> SubcategoryByNameAndCategoryExpression(string? name,
        int categoryId) =>
        subcategory => subcategory.Name == name && subcategory.CategoryId == categoryId;

    private Category? GetExistingCategory(string categoryName)
    {
        return context.Categories.Local.SingleOrDefault(c => c.Name == categoryName);
    }
}