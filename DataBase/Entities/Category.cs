namespace DataBase.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Subcategory> Subcategories { get; set; } = [];
}