using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Receipts.DataBase.Entities;

[Index(nameof(Fd), nameof(S), nameof(Fn), nameof(Fp), nameof(PurchaseDate), IsUnique = true)]
class Check
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
    public Guid UserId { get; set; }
    public Guid BudgetId { get; set; }
    public override string ToString()
    {
        return $"""
                Продукты:
                {string.Join('\n', Products)}
                """;
    }
}