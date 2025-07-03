namespace Fns.Contracts.Categorization;

public interface IProductCategorizationService
{
    public Task<CategorizationResponse> CategorizeProducts(CategorizationRequest request);
}

public record CategorizationRequest(IReadOnlyList<CategorizationProduct> Products);
public record CategorizationProduct(string Name);

public record CategorizationResponse(IReadOnlyList<CategorizedProduct> Products);
public record CategorizedProduct(string Name, string? Subcategory, string Category);