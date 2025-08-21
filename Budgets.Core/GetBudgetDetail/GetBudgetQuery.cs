using System.Security.Claims;
using Budgets.Contracts.GetBudgetDetail;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.GetBudgetDetail;

public sealed record GetBudgetQuery(Guid Id, ClaimsPrincipal User): IResultQuery<BudgetFullDetail>;