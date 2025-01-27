using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

[Index(nameof(Fd), nameof(Fn), nameof(Fp), nameof(PurchaseDate), IsUnique = true)]
public class Check
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime AddedDate { get; set; }
    public string Fd { get; set; }
    public string Fn { get; set; }
    public string Fp { get; set; }
    public string S { get; set; }
    // public string CheckRaw { get; set; } = null!;
    [MinLength(1)]
    public List<Product> Products { get; set; } = [];

    public override string ToString() => 
    $"""
    Продукты:
    {string.Join('\n', Products)}
    """
    ;
}