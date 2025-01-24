using Core.Model;

namespace Core.Mappers;

public static class CategoryMappers
{
    public static Category ConvertToCategory(this DataBase.Entities.Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
    };
}