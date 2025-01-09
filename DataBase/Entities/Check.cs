using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

public class Check
{
    public Guid Id { get; set; }
    public int Sum { get; set; }
    public DateTime Date { get; set; }
    [MinLength(1)]
    public List<Product> Products { get; set; } = [];
}