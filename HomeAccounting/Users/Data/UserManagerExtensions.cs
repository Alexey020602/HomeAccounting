using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Users.Data;

static class UserManagerExtensions
{
    public static Task<User?> FindByIdAsync(this UserManager<User> userManager, UserId id) => userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
}