using System.ComponentModel.DataAnnotations;

namespace Budgets.Core.Model;
public class Budget
{
    public Guid Id { get; set; }
    [StringLength(100)] [Required] public string? Name { get; set; }
    public int BeginOfPeriod { get; set; }
    public int? Limit { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public List<BudgetUser> BudgetUsers { get; set; } = [];
}