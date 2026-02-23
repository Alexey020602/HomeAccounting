using System.Net;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Budgets;

internal sealed class BudgetRequirementsAuthorizationHandler(BudgetsContext budgetsContext): AuthorizationHandler<BudgetRequirements, BudgetId>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BudgetRequirements requirement, BudgetId budgetId)
    {
        if (requirement.Permission == BudgetPermissions.Undefined)
        {
            context.Fail(new AuthorizationFailureReason(this, "User permissions not set"));
        }
        
        var userId = new UserId(context.User.GetUserId());

        var budget = await budgetsContext.Budgets
            .Include(b => b.BudgetUsers)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget is null)
        {
            throw new DomainException("Budget not Exists");
        }

        var budgetRoleId = budget.GetUserRole(userId);
        if (budgetRoleId is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "User not added in budget"));
            return;
        }

        var userRole = await budgetsContext.BudgetRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == budgetRoleId);

        if (userRole is null)
        {
            throw new DomainException("Role not exists");
        }
        
        if (!userRole.Permissions.HasFlag(requirement.Permission))
        {
            context.Fail(new AuthorizationFailureReason(this, "User has no required permission"));
            return;
        }
        
        context.Succeed(requirement);
    }
}

internal sealed class DomainException(string? message = null): Exception(message);