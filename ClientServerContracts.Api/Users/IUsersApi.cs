using ClientServerContracts.Users.GetUser;
using ClientServerContracts.Users.UpdateUser;
using Refit;

namespace ClientServerContracts.Api.Users;

/// <summary>
/// Refit client for Users API endpoints.
/// </summary>
[Headers("Authorization: Bearer")]
public interface IUsersApi
{
    /// <summary>
    /// Returns user by guid.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>User data.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved user.</description></item>
    /// <item><term>404</term><description>Not Found - User not found.</description></item>
    /// </list>
    /// </remarks>
    [Get("/users/{id}")]
    public Task<User> GetUser(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns user by username.
    /// </summary>
    /// <param name="username">Username.</param>
    /// <returns>User data.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved user.</description></item>
    /// <item><term>404</term><description>Not Found - User not found.</description></item>
    /// </list>
    /// </remarks>
    [Get("/users/{username}")]
    public Task<User> GetUserByUsername(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user full name and username by id.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="user">Updated user data.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - User successfully updated.</description></item>
    /// <item><term>404</term><description>Not Found - User not found.</description></item>
    /// <item><term>400</term><description>Bad Request - Validation errors.</description></item>
    /// </list>
    /// </remarks>
    [Put("/users/{id}")]
    public Task UpdateUser(Guid id, UpdatedUserDto user, CancellationToken cancellationToken = default);
}
