using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;
[Index(nameof(Name), nameof(CategoryId), IsUnique = true)]
public class Subcategory
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<Product> Products { get; set; } = [];
}   