namespace Budgets.Core.UserInBudgetPermissions;

[Flags]
public enum BudgetPermissions
{
    Undefined = 0,
    Read = 1 << 0,
    Edit = 1 << 1,
    Delete = 1 << 2,
}

public static class BudgetPermissionsExtensions
{
    public static bool Contains(this BudgetPermissions permissions, BudgetPermissions permission) =>
        (permissions & permission) == permission;
}