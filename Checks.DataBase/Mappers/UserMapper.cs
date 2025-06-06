using System.Security.Claims;
using Checks.DataBase.Entities;
using User = Authorization.Contracts.User;

namespace Checks.DataBase.Mappers;
using CoreUser = User;
public static class UserMapper
{
    public static Entities.User ToEntity(this CoreUser model) => new Entities.User()
    {
        Id = model.Login,
        UserName = model.Name,
    };
    
    public static CoreUser CreateUser(this Entities.User user) => new(user.Id, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName), user.UserName));
}