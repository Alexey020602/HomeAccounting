using System.ComponentModel.DataAnnotations;

namespace Budgets.DataBase.Entities;

internal class Budget
{
    public Guid Id { get; set; }
    [StringLength(100)] public string Name { get; set; } = null!;
}