using System.ComponentModel.DataAnnotations;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase.Entities;

[Index(nameof(Name), IsUnique = true)]
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