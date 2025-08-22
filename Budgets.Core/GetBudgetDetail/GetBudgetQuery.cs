using System.Security.Claims;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.GetBudgetDetail;

public sealed record GetBudgetQuery(Guid Id, ClaimsPrincipal User): UserBudgetMessage(Id, User, BudgetPermissions.Read), IResultQuery<BudgetFullDetail>;