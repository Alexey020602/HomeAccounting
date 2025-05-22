using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public List<Subcategory> Subcategories { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj) => obj is Category category && category.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
}