using User = Authorization.Contracts.User;

namespace Authorization;
using CoreUser = Contracts.User;
public static class UserMapper
{
    public static User ToEntity(this CoreUser model) => new User()
    {
        Id = model.Login,
        UserName = model.Name,
    };
    
    public static CoreUser CreateUser(this User user) => new(user.Id, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName), user.UserName));
}