using Authorization.Contracts;
using Budgets.Core.Model;
using MaybeResults;
using Shared.Utils.MediatorWithResults;
using BudgetUser = Budgets.Contracts.GetBudgetDetail.BudgetUser;

namespace Budgets.Core.GetBudgetUsers;

public sealed class GetBudgetUsersQueryHandler(
    IGetBudgetUsersService getBudgetUsersService,
    IUsersService usersService
) : IResultQueryHandler<GetBudgetUsersQuery, IReadOnlyCollection<BudgetUser>>
{
    public async ValueTask<IMaybe<IReadOnlyCollection<BudgetUser>>> Handle(
        GetBudgetUsersQuery query,
        CancellationToken cancellationToken
    ) => await GetBudgetUsers(
        await getBudgetUsersService.GetUsersForBudget(query.BudgetId)
    );

    private async Task<IMaybe<IReadOnlyCollection<Contracts.GetBudgetDetail.BudgetUser>>> GetBudgetUsers(
        IReadOnlyCollection<Model.BudgetUser> budgetUsers)
    {
        return from users in await usersService
                .GetUsers(new(
                    budgetUsers.Select(u => u.UserId).ToArray()
                ))
            select MergeUsers(budgetUsers, users) as IReadOnlyCollection<Contracts.GetBudgetDetail.BudgetUser>;
    }

    private static List<Contracts.GetBudgetDetail.BudgetUser> MergeUsers(
        IEnumerable<Model.BudgetUser> budgetUsers,
        IEnumerable<User> users) =>
        (
            from user in users
            from budgetUser in budgetUsers
            where user.Id == budgetUser.UserId
            select CreateUser(user, budgetUser)
        )
        .ToList();

    private static Contracts.GetBudgetDetail.BudgetUser CreateUser(User user, Model.BudgetUser budgetUser) =>
        new(
            user.Id,
            user.UserName,
            budgetUser.BudgetRole is not null
                ? budgetUser.BudgetRole.Name ?? throw new InvalidOperationException(
                    $"{nameof(BudgetUser)}.{nameof(Model.BudgetUser.BudgetRole)}.{nameof(Model.BudgetUser.BudgetRole.Name)} must have a value")
                : throw new InvalidOperationException(
                    $"{nameof(BudgetUser)} must have loaded {nameof(BudgetRole)} property"));
}