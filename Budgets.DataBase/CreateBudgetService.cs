using Budgets.Core.CreateBudget;
using Budgets.Core.Model;
using Budgets.DataBase.Entities;
using MaybeResults;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class CreateBudgetService(BudgetsContext dbContext): ICreateBudgetService
{
    public async Task<IMaybe> CreateAsync(NewBudget budget)
    {
        if (await dbContext.BudgetRoles.SingleOrDefaultAsync(role => role.Name == BudgetRole.OwnerRoleName) is not
            { } ownerRole)
        {
            throw new InvalidOperationException("Not found owner role for budget");
        }
        
        dbContext.Budgets.Add(new Budget()
        {
            BeginOfPeriod = budget.BudgetData.BeginOfPeriod,
            CreatorId = budget.UserId,
            Name = budget.BudgetData.Name,
            Limit = budget.BudgetData.Limit,
            CreationDate = budget.CreationDate,
            BudgetUsers = [
                new()
                {
                    UserId = budget.UserId,
                    BudgetRoleId = ownerRole.Id,
                }
            ]
        });
        
        await dbContext.SaveChangesAsync();
        return Maybe.Create();
    }
}