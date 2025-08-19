using Budgets.DataBase.Entities;
using Microsoft.AspNetCore.Authorization;
using Shared.Utils.Model;
using Budget = Budgets.Contracts.GetBudgets.Budget;

namespace Budgets.Api;

internal sealed record ReadBudgetRequirement() : IAuthorizationRequirement;

internal sealed class ReadBudgetRequirementHandler : AuthorizationHandler<ReadBudgetRequirement, BudgetUser>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ReadBudgetRequirement requirement, 
        BudgetUser resource)
    {
        var userId = context.User.GetUserId();
        
        
    }
}

