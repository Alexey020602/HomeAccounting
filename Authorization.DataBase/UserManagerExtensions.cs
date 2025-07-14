using System.Linq.Expressions;
using Authorization.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

public static class UserManagerExtensions
{
    public static Task<Core.User?> FindByRefreshTokenAsync(
        this UserManager<User> userManager,
        string refreshToken) => userManager.Users.FirstOrDefaultAsync(FindByRefreshTokenExpression(refreshToken));
    public static Core.User? FindByRefreshToken(
        this UserManager<User> userManager,
        string refreshToken) => userManager.Users.FirstOrDefault(FindByRefreshTokenExpression(refreshToken));
    private static Expression<Func<Core.User, bool>> FindByRefreshTokenExpression(string refreshToken) =>
        user =>
            user.RefreshToken != null
            && user.RefreshToken.Token == refreshToken;
}