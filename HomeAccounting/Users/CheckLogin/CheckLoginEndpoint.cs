using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.CheckLogin;

static class CheckLoginEndpoint
{
    public static void MapCheckLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "login/exist",
            async (string login, UserManager<User> userManager) => 
                Results.Ok((await userManager.FindByNameAsync(login) != null)))
            .AllowAnonymous();
    }
}