using System.ComponentModel.DataAnnotations;
using Budgets.Core.UserInBudgetPermissions;

namespace Budgets.Core.Model;

public class BudgetRole
{
    public const string OwnerRoleName = "Владелец";
    public const string AdminRoleName = "Администратор";
    public const string UserRoleName = "Пользователь";
    public Guid Id { get; set; }
    [StringLength(100)] [Required] public string? Name { get; set; }
    public BudgetPermissions Permissions { get; set; }
    public List<BudgetUser> BudgetUsers { get; set; } = [];
}