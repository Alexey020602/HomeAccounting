using HomeAccounting.Users.CheckLogin;
using HomeAccounting.Users.GetUser;
using HomeAccounting.Users.Login;
using HomeAccounting.Users.RefreshToken;
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
        
        var usersGroup = endpoints.MapGroup("users");
        usersGroup.MapGetUser();
        usersGroup.MapUpdateUserEndpoint();
    }
}