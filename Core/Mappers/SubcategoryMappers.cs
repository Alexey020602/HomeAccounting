using Core.Model;

namespace Core.Mappers;

public static class SubcategoryMappers
{
    public static Subcategory ConvertToSubcategory(this DataBase.Entities.Subcategory subcategory)
    {
        return new Subcategory
        {
            Id = subcategory.Id,
            Name = subcategory.Name,
            Category = subcategory.Category.ConvertToCategory()
        };
    }
}