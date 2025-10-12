using Microsoft.AspNetCore.Identity;
using MyBudgets.Users.Data;

namespace MyBudgets.Users.CheckLogin;

static class CheckLoginEndpoint
{
    public static void MapCheckLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "login/exist",
            async (string login, UserManager<User> userManager) => 
                Results.Ok((await userManager.FindByNameAsync(login) != null)));
    }
}