using Authorization.Core;
using Authorization.Core.Registration;
using MaybeResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

// public static class UserMapper
// {
//     public static Core.User ConvertToUser(this User user)
//     {
//         if (user.UserName == null)
//         {
//             throw new UserException("UserName not found");
//         }
//
//         if (user.RefreshTokenToken == null)
//         {
//             throw new UserException("RefreshToken not found");
//         }
//         return new Core.User()
//         {
//             Login = user.Id,
//             Username = user.UserName,
//             RefreshToken = user.RefreshTokenToken.ConvertToCoreRefreshToken(),
//         };
//     }
//
//     public static User ConvertToUser(this Core.User user) => new User()
//     {
//         Id = user.Login,
//         UserName = user.Username,
//         RefreshTokenToken = user.RefreshToken?.ConvertToRefreshToken(),
//     };
// }

public class UserService(UserManager<User> userManager): IUserService
{
    public async Task<bool> CheckLoginExist(string login, CancellationToken cancellation = default)
    {
        return await userManager.FindByNameAsync(login) != null;
    }

    public async Task<IMaybe<User>> GetById(Guid id, CancellationToken cancellation = default) => 
        await userManager.FindByIdAsync(id.ToString()) is {} user 
            ? Maybe.Create(user) 
            : new UserNotFoundError<User>("User not found");

    public async Task<IMaybe<User>> GetUserByRequest(UserRequest request,
        Func<RefreshToken> createRefreshToken,
        CancellationToken cancellation = default)
    {
        var user = await userManager.FindByNameAsync(request.Login);
        // var user = await userService.GetUserByLogin(loginRequest.Login);

        if (user is null) return new UserNotFoundError<User>("User not found");

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            return new UserError<User>("Wrong Password");
        
        user.RefreshToken = RefreshTokenMapper.ConvertToRefreshToken(createRefreshToken());
        //todo Добавить обработку ошибок
        await userManager.UpdateAsync(user);
        
        return Maybe.Create(user);
    }

    public async Task<IMaybe> UpdateUser(User user, CancellationToken cancellation = default)
    {
        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Maybe.Create();
        }
        
        return new UserError("Error while updating user", result);
    } 

    public async Task<IMaybe<User>> GetUserByRefreshToken(
        string refreshToken,
        Func<RefreshToken> createRefreshToken, 
        CancellationToken cancellation = default
        )
    {
        var user = await userManager.Users.FirstOrDefaultAsync(
            user => user.RefreshToken != null && user.RefreshToken.Token == refreshToken, 
            cancellation);
        if (user is null)
        {
            return new UserNotFoundError<User>("User Not Found");
        }

        if (user.RefreshToken is null || user.RefreshToken.Expires < DateTime.UtcNow)
        {
            return new RefreshTokenError<User>("Refresh token is expired or not exists");
        }
        
        user.RefreshToken = RefreshTokenMapper.ConvertToRefreshToken(createRefreshToken());
        
        await userManager.UpdateAsync(user);

        return Maybe.Create(user);
    }

    public async Task<IMaybe> AddUser(UnregisteredUser user, string password, CancellationToken cancellation = default)
    {
        var existingUser = await userManager.FindByNameAsync(user.Login);
        if (existingUser is not null) return new UserError("User already exists");

        var creationResult = await userManager.CreateAsync(
            new User
            {
                UserName = user.Login,
                FullName = user.UserName,
            },
            password
        );

        if (creationResult.Succeeded)
        {
            return Maybe.Create();
        }

        return new UserError("User creation failed", creationResult);
    }
}