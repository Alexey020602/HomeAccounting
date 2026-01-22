using Microsoft.EntityFrameworkCore;
using MyBudgets.Users.Data;
using MyBudgets.Users.Data.Database;

namespace MyBudgets.Users.GetUsers;

internal sealed class UsersService(UsersContext usersContext) : IUsersService
{
    public async Task<IEnumerable<UserDto>> GetUsers(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var userIds = ids.Select(x => new UserId(x)).ToArray();
        return await usersContext.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u=>u.ConvertToDto())
            .ToListAsync(cancellationToken);
    }
}