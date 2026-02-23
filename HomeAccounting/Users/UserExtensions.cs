using ClientServerShared.Users;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users;

internal static class UserExtensions
{
    extension(User user)
    {
        public UserDto ConvertToDto()
        {
            return new(user.Id.Value, user.UserName ?? throw UserException.NoUserName, user.FullName);
        }
    }
}