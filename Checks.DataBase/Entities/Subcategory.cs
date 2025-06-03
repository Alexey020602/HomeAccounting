using Microsoft.EntityFrameworkCore;

namespace Checks.DataBase.Entities;

[Index(nameof(Name), nameof(CategoryId), IsUnique = true)]
public class Subcategory
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<Product> Products { get; set; } = [];

    public override string ToString()
    {
        return $"{Name} из {Category}";
    }

    public override bool Equals(object? obj) => obj is Subcategory subcategory && subcategory.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
}