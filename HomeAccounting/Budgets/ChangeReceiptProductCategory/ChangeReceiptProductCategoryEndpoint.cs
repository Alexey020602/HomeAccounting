using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.ChangeReceiptProductCategory;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Categories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.ChangeReceiptProductCategory;

static class ChangeReceiptProductCategoryEndpoint
{
    public static void MapChangeReceiptProductCategory(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "{budgetId:guid}/receipts/{receiptId:guid}/products/{productId:guid}/category",
            async (Guid budgetId, Guid receiptId, Guid productId, ChangeReceiptProductCategoryRequest request,
                ClaimsPrincipal user, BudgetsContext context, IAuthorizationService authorization, CancellationToken ct) =>
            {
                var budgetIdTyped = new BudgetId(budgetId);
                var result = await authorization.AuthorizeAsync(user, budgetIdTyped, new BudgetRequirements(BudgetPermissions.Edit));
                if (!result.Succeeded)
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to edit this budget");

                var receipt = await context.Receipts
                    .Include(r => r.Products)
                    .FirstOrDefaultAsync(r => r.Id == new ReceiptId(receiptId), ct);
                if (receipt is null || receipt.BudgetId != budgetIdTyped)
                    return Results.NotFound();

                receipt.ChangeCategoryForProduct(new ProductId(productId), new CategoryId(request.CategoryId));
                await context.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .WithName("ChangeReceiptProductCategory")
            .WithTags("Budgets")
            .WithSummary("Change receipt product category")
            .WithDescription("Sets category for a product in a receipt. Requires edit permission.")
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }
}
