using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Entities;

[Table(nameof(Subcategory))]
public class Subcategory: Category
{
    public string SubcategoryName { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public List<Product> Products { get; set; } = [];
}   