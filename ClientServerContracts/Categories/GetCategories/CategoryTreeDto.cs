namespace ClientServerContracts.Categories.GetCategories;

public sealed record CategoryTreeDto(int Id, string Name, int? ParentCategoryId, IReadOnlyList<CategoryTreeDto> Children);

