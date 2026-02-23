using Microsoft.EntityFrameworkCore;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;

namespace HomeAccounting.Budgets.Data.Database.Seeding;

internal static class BudgetsContextSeedingExtensions
{
    extension(BudgetsContext budgetsContext)
    {
        internal void AddBudgetRoles()
        {
            foreach (var budgetRole in BudgetRole.GetDefaultRoles())
            {
                if (budgetsContext.BudgetRoles.SingleOrDefault(b => b.Id == budgetRole.Id) is not null)
                    continue;
                budgetsContext.BudgetRoles.Add(budgetRole);
            }
        }
        public async Task AddBudgetRolesAsync(CancellationToken cancellationToken)
        {
            foreach (var budget in BudgetRole.GetDefaultRoles())
            {
                if (await budgetsContext.BudgetRoles.SingleOrDefaultAsync(r => r.Id == budget.Id,
                        cancellationToken: cancellationToken) is not null)
                    continue;

                budgetsContext.BudgetRoles.Add(budget);
            }
        }

        public void AddBudgets()
        {
            foreach (var budget in Budget.GetDefaultBudgets())
            {
                if (budgetsContext.Budgets.SingleOrDefault(b => b.Id == budget.Id) is not null)
                    continue;

                budgetsContext.Budgets.Add(budget);
            }
        }

        public async Task AddBudgetsAsync(CancellationToken cancellationToken)
        {
            foreach (var budget in Budget.GetDefaultBudgets())
            {
                if (await budgetsContext.Budgets.SingleOrDefaultAsync(b => b.Id == budget.Id,
                        cancellationToken: cancellationToken) is not null)
                    continue;

                budgetsContext.Budgets.Add(budget);
            }
        }

        public void AddReceipts()
        {
            foreach (var receipt in Receipt.GetDefaultReceiptsForFirstBudget().Concat(Receipt.GetDefaultReceiptsForSecondBudget()))
            {
                if (budgetsContext.Receipts.SingleOrDefault(r => r.Id == receipt.Id) is not null)
                    continue;
                budgetsContext.Receipts.Add(receipt);
            }
        }

        public async Task AddReceiptsAsync(CancellationToken cancellationToken)
        {
            foreach (var receipt in Receipt.GetDefaultReceiptsForFirstBudget().Concat(Receipt.GetDefaultReceiptsForSecondBudget()))
            {
                if (await budgetsContext.Receipts.SingleOrDefaultAsync(r => r.Id == receipt.Id, cancellationToken) is not null)
                    continue;
                budgetsContext.Receipts.Add(receipt);
            }
        }
    }
}




