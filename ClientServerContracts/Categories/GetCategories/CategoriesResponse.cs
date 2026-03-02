namespace ClientServerContracts.Categories.GetCategories;

public sealed record CategoriesResponse(
    IReadOnlyCollection<CategoryDto> Categories,
    IReadOnlyCollection<CategoriesPathItem> Path);