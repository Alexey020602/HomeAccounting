using System.Security.Claims;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.GetBudgetUsers;

public record GetBudgetUsersQuery(Guid BudgetId, ClaimsPrincipal User): 
    UserBudgetMessage(BudgetId, User, BudgetPermissions.Read), 
    IResultQuery<IReadOnlyCollection<BudgetUser>>;