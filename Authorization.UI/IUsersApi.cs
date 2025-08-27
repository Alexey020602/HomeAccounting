using Authorization.Contracts;
using Refit;
using Shared.Blazor.Attributes;

namespace Authorization.UI;

[ApiAuthorizable("users")]
[Headers("Authorization: Bearer")]
public interface IUsersApi
{
    [Get("/{id}")] public Task<User> GetUser(Guid id);
    [Put("/{id}")] public Task UpdateUser(Guid id, UpdatedUserDto user);
}