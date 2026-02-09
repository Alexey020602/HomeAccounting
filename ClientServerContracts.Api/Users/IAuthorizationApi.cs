using ClientServerContracts.Api.Attributes;
using ClientServerContracts.Users.Login;
using ClientServerContracts.Users.Register;
using Refit;

namespace ClientServerContracts.Api.Users;

/// <summary>
/// Query parameters for CheckLoginExist endpoint.
/// </summary>
public sealed class CheckLoginExistQueryParameters
{
    /// <summary>
    /// Login (username) to check.
    /// </summary>
    public string Login { get; set; } = string.Empty;
}

/// <summary>
/// Refit client for Authorization API endpoints.
/// </summary>
[Api]
public interface IAuthorizationApi
{
    /// <summary>
    /// Returns true if the login (username) is already taken. Used for registration validation.
    /// </summary>
    /// <param name="query">Query parameters.</param>
    /// <returns>True if login exists, false otherwise.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully checked login existence.</description></item>
    /// </list>
    /// </remarks>
    [Get("/login/exist")]
    Task<bool> CheckLoginExist([Query] CheckLoginExistQueryParameters query);

    /// <summary>
    /// Authenticates user by login and password. Returns JWT and refresh token.
    /// </summary>
    /// <param name="loginRequest">Login credentials.</param>
    /// <returns>Authorization response with JWT and refresh token.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully authenticated.</description></item>
    /// <item><term>404</term><description>Not Found - User not found.</description></item>
    /// <item><term>400</term><description>Bad Request - Wrong password.</description></item>
    /// </list>
    /// </remarks>
    [Post("/login")]
    Task<AuthorizationResponse> Login(LoginRequest loginRequest);

    /// <summary>
    /// Creates a new user account with the provided credentials.
    /// </summary>
    /// <param name="registrationRequest">Registration data.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - User successfully registered.</description></item>
    /// <item><term>400</term><description>Bad Request - User with username already exists or validation errors.</description></item>
    /// </list>
    /// </remarks>
    [Post("/register")]
    Task Register(RegistrationRequest registrationRequest);

    /// <summary>
    /// Exchanges a valid refresh token for new JWT and refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token string.</param>
    /// <returns>Authorization response with new JWT and refresh token.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully refreshed token.</description></item>
    /// <item><term>404</term><description>Not Found - User not found.</description></item>
    /// <item><term>401</term><description>Unauthorized - Refresh token expired or invalid.</description></item>
    /// </list>
    /// </remarks>
    [Post("/refresh")]
    Task<AuthorizationResponse> RefreshToken([Body] string refreshToken);
}
