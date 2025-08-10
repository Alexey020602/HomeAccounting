using System.Linq.Expressions;
using Budgets.Core.GetBudgets;
using Budgets.DataBase.Entities;

namespace Budgets.DataBase;

internal static class BudgetsRequestExtensions
{
    public static Expression<Func<BudgetUser, bool>> FilterExpression(this BudgetsRequest budgetsRequest) => budget => 
        budget.UserId == budgetsRequest.UserId;
}