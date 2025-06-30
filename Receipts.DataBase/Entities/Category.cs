using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Receipts.DataBase.Entities;

[Index(nameof(Name), IsUnique = true)]
class Category
{
    private const string DefaultName = "Unitialized";
    public int Id { get; init; }
    public string Name { get; set; } = DefaultName;
    public List<Subcategory> Subcategories { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj) => obj is Category category && category.Id == Id;
    internal static Category Default => new () { Name = DefaultName };
    public override int GetHashCode() => Id.GetHashCode();
}