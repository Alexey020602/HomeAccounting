using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Subcategory> Subcategories { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }
}