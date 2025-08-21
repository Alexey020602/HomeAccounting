using Authorization.Contracts;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Core.Model;
using Budgets.Core.UserInBudgetPermissions;
using MaybeResults;
using Microsoft.AspNetCore.Authorization;
using Shared.Utils.MediatorWithResults;
using BudgetUser = Budgets.Core.Model.BudgetUser;

namespace Budgets.Core.GetBudgetDetail;

public sealed class GetBudgetDetailHandler(
    IGetBudgetDetailService service,
    IUsersService usersService,
    IAuthorizationService authorizationService)
    : IResultQueryHandler<GetBudgetQuery, BudgetFullDetail>
{
    public async ValueTask<IMaybe<BudgetFullDetail>> Handle(GetBudgetQuery query, CancellationToken cancellationToken)
    {
        if (await authorizationService.CheckUserHasPermission(query.User, query.Id, BudgetPermissions.Read) is INone
            permissionError)
        {
            return permissionError.Cast<BudgetFullDetail>();
        }

        return await (
            from budget in service.GetBudgetFullDetail(query.Id, cancellationToken)
            from users in GetBudgetUsers(budget.BudgetUsers ?? [])
            select new BudgetFullDetail(
                budget.Id,
                budget.Name ?? throw new InvalidOperationException(),
                budget.Limit,
                budget.BeginOfPeriod,
                users
            )
        );
    }

    private async Task<IMaybe<IReadOnlyCollection<Contracts.GetBudgetDetail.BudgetUser>>> GetBudgetUsers(
        IReadOnlyCollection<BudgetUser> budgetUsers)
    {
        return from users in await usersService
                .GetUsers(new(
                    budgetUsers.Select(u => u.UserId).ToArray()
                ))
            select MergeUsers(budgetUsers, users);
    }

    private static List<Contracts.GetBudgetDetail.BudgetUser> MergeUsers(
        IEnumerable<BudgetUser> budgetUsers,
        IEnumerable<User> users) =>
        (
            from user in users
            from budgetUser in budgetUsers
            where user.Id == budgetUser.UserId
            select CreateUser(user, budgetUser)
        )
        .ToList();

    private static Contracts.GetBudgetDetail.BudgetUser CreateUser(User user, BudgetUser budgetUser) =>
        new(
            user.Id, 
            user.UserName, 
            budgetUser.BudgetRole is not null  
                ? budgetUser.BudgetRole.Name ?? throw new InvalidOperationException($"{nameof(BudgetUser)}.{nameof(BudgetUser.BudgetRole)}.{nameof(BudgetUser.BudgetRole.Name)} must have a value")
                : throw new InvalidOperationException($"{nameof(BudgetUser)} must have loaded {nameof(BudgetRole.Name)} property"));
}

