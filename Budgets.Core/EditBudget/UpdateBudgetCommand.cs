using System.Security.Claims;
using Budgets.Contracts;
using Budgets.Core.CreateBudget;
using Budgets.Core.UserInBudgetPermissions;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.EditBudget;

public record UpdateBudgetCommand(Guid BudgetId, ClaimsPrincipal User, BudgetData BudgetData)
    : UserBudgetMessage(BudgetId, User, BudgetPermissions.Edit), IResultCommand;