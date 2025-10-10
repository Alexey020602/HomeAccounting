using BlazorConsolidated.Common.Attributes;
using ClientServerContracts.User.GetUser;
using ClientServerContracts.User.UpdateUser;
using Refit;

namespace BlazorConsolidated.Users;

[ApiAuthorizable("users")]
[Headers("Authorization: Bearer")]
public interface IUsersApi
{
    [Get("/{id}")] public Task<User> GetUser(Guid id);
    [Put("/{id}")] public Task UpdateUser(Guid id, UpdatedUserDto user);
}