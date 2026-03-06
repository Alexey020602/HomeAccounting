namespace ClientServerContracts.Categories.GetCategoriesTree;

public sealed record CategoryTreeDto(int Id, string Name, int? ParentCategoryId, IReadOnlyList<CategoryTreeDto> Children);

