using HomeAccounting.Users.CheckLogin;
using HomeAccounting.Users.GetUser;
using HomeAccounting.Users.Login;
using HomeAccounting.Users.Logout;
using HomeAccounting.Users.Refresh;
using HomeAccounting.Users.Register;
using HomeAccounting.Users.UpdateUser;

namespace HomeAccounting.Users;

static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapLogin();
        endpoints.MapRegister();
        endpoints.MapCheckLogin();
        endpoints.MapRefreshToken();
        endpoints.MapLogout();
        endpoints.MapLogoutAll();

        var usersGroup = endpoints.MapGroup("users");
        usersGroup.MapGetUser();
        usersGroup.MapUpdateUserEndpoint();
    }
}