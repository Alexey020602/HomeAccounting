using ClientServerContracts.Budgets.GetBudgetSpendings;

namespace ClientServerContracts.Budgets.GetBudgetDetail;

public sealed record  GetBudgetsDetailResponse(Guid Id,  string Name,  int BeginOfPeriod, int? Limit);