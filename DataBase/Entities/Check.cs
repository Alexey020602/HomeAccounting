using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

[Index(nameof(CheckRaw), IsUnique = true)]
public class Check
{
    public Guid Id { get; set; }
    public int Sum { get; set; }
    public DateTime Date { get; set; }
    public string CheckRaw { get; set; } = null!;
    [MinLength(1)]
    public List<Product> Products { get; set; } = [];

    public override string ToString() => 
    $"""
    Продукты:
    {string.Join('\n', Products)}
    """
    ;
}