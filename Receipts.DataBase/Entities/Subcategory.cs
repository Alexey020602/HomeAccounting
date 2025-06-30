using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Receipts.DataBase.Entities;

[Index(nameof(Name), nameof(CategoryId), IsUnique = true)]
class Subcategory
{
    private const string DefaultName = "Unitialized";
    public int Id { get; init; }
    public string? Name { get; set; } = DefaultName;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = Category.Default;
    public List<Product> Products { get; set; } = [];

    public override string ToString()
    {
        return $"{Name} из {Category}";
    }

    public override bool Equals(object? obj) => obj is Subcategory subcategory && subcategory.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
    internal static Subcategory Default => new ()
    {
        Name = DefaultName,
    };
}