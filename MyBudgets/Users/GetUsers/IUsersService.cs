namespace MyBudgets.Users.GetUsers;

internal interface IUsersService
{
    Task<IEnumerable<UserDto>> GetUsers(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}