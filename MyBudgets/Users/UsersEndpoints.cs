using MyBudgets.Users.CheckLogin;
using MyBudgets.Users.GetUser;
using MyBudgets.Users.Login;
using MyBudgets.Users.RefreshToken;
using MyBudgets.Users.Register;
using MyBudgets.Users.UpdateUser;

namespace MyBudgets.Users;

static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapLogin();
        endpoints.MapRegister();
        endpoints.MapCheckLogin();
        endpoints.MapRefreshToken();
        
        var usersGroup = endpoints.MapGroup("users").RequireAuthorization();
        usersGroup.MapGetUser();
        usersGroup.MapUpdateUserEndpoint();
    }
}