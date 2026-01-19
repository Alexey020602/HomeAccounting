using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

class BudgetRole
{
    public static readonly BudgetRoleId OwnerBudgetRoleId = new BudgetRoleId(1);
    public static readonly BudgetRoleId AdminBudgetRoleId = new BudgetRoleId(2);
    public static readonly BudgetRoleId UserBudgetRoleId = new BudgetRoleId(3);
    public const string OwnerRoleName = "Владелец";
    public const string AdminRoleName = "Администратор";
    public const string UserRoleName = "Пользователь";
    public BudgetRoleId Id { get; private set; }
    public string Name { get; private set; }
    public BudgetPermissions Permissions { get; private set; }
    internal BudgetRole()
    {
        Name = string.Empty;
    }

    public BudgetRole(string name,  BudgetPermissions permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public bool CanUserEdit => Permissions.HasFlag(BudgetPermissions.Edit);
    public bool CanUserDelete => Permissions.HasFlag(BudgetPermissions.Delete);
    public bool CanUserRead => Permissions.HasFlag(BudgetPermissions.Read);

    public static IEnumerable<BudgetRole> GetDefaultRoles() =>
    [
        new(
            OwnerRoleName, 
            BudgetPermissions.Read | BudgetPermissions.Edit | BudgetPermissions.Delete)
        {
            Id = OwnerBudgetRoleId,
        },
        new (
            AdminRoleName,
            BudgetPermissions.Read | BudgetPermissions.Edit)
        {
            Id = AdminBudgetRoleId,
        },
        new (
            UserRoleName,
            BudgetPermissions.Read)
        {
            Id =  UserBudgetRoleId,
        },
    ];
}