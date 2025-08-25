using MaybeResults;
using Shared.Utils.Results;

namespace Budgets.Core.UserInBudgetPermissions;


[None]
public partial record UserHasNoPermission : IForbiddenError;
public partial record UserHasNoPermission<T> : IForbiddenError;