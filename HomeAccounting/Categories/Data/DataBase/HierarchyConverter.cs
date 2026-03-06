using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeAccounting.Categories.Data;

internal sealed class HierarchyConverter(): ValueConverter<CategoryHierarchy, string>(
    h=>h.Path,
    x=> new CategoryHierarchy(x)
);