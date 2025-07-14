using Authorization.Core;
using Authorization.Core.Registration;
using LightResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

sealed class UserException(string message) : Exception(message);
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
    public async Task<bool> CheckLoginExist(string login)
    {
        return await userManager.FindByIdAsync(login) != null;
    }

    public async Task<Result<Core.User>> GetUserByRequest(
        UserRequest request, 
        Func<Core.RefreshToken> createRefreshToken
        )
    {
        var user = await userManager.FindByIdAsync(request.Login);
        // var user = await userService.GetUserByLogin(loginRequest.Login);

        if (user is null) return Result.Failure<Core.User>("User not found");

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<Core.User>("Wrong Password");
        
        user.RefreshToken = RefreshTokenMapper.ConvertToRefreshToken(createRefreshToken());
        //todo Добавить обработку ошибок
        await userManager.UpdateAsync(user);
        
        return user;
    }

    public async Task<Result> UpdateUser(Core.User user)
    {
        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Result.Success();
        }
        
        return Result.Failure(ConvertToErrors(result));
    } 

    public async Task<Result<Core.User>> GetUserByRefreshToken(
        string refreshToken,
        Func<Core.RefreshToken> createRefreshToken
        )
    {
        var user = await userManager.Users.FirstOrDefaultAsync(user =>
            user.RefreshToken != null && user.RefreshToken.Token == refreshToken);
        if (user is null)
        {
            return Result.Failure<Core.User>("User Not Found");
        }

        if (user.RefreshToken is null || user.RefreshToken.Expires < DateTime.UtcNow)
        {
            return Result.Failure<Core.User>(new RefreshTokenError());
        }
        
        user.RefreshToken = RefreshTokenMapper.ConvertToRefreshToken(createRefreshToken());
        
        await userManager.UpdateAsync(user);

        return user;
    }

    public async Task<Result> AddUser(UnregisteredUser user, string password)
    {
        var existingUser = await userManager.FindByIdAsync(user.Login);
        if (existingUser is not null) return Result.Failure("User already exists");

        var creationResult = await userManager.CreateAsync(
            new User
            {
                Id = user.Login,
                UserName = user.UserName,
            },
            password
        );

        if (creationResult.Succeeded)
        {
            return Result.Success();
        }

        return Result.Failure(ConvertToErrors(creationResult));
    }
    private static IEnumerable<IError> ConvertToErrors(IdentityResult identityResult) =>
        identityResult.Errors.Select(e => new UserCreationError(e));
}