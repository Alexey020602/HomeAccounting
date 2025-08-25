using System.Security.Claims;
using Budgets.Core.UserInBudgetPermissions;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.DeleteBudget;

public sealed record DeleteBudgetCommand(Guid BudgetId, ClaimsPrincipal User) : UserBudgetMessage(BudgetId, User, BudgetPermissions.Delete), IResultCommand;