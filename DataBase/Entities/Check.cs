using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

[Index(nameof(Fd), nameof(S), nameof(Fn), nameof(Fp), nameof(PurchaseDate), IsUnique = true)]
public class Check
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime AddedDate { get; set; }
    public string Fd { get; set; } = null!;
    public string Fn { get; set; } = null!;
    public string Fp { get; set; } = null!;

    public string S { get; set; } = null!;

    // public string CheckRaw { get; set; } = null!;
    [MinLength(1)] public List<Product> Products { get; set; } = [];

    public override string ToString()
    {
        return $"""
                Продукты:
                {string.Join('\n', Products)}
                """;
    }
}