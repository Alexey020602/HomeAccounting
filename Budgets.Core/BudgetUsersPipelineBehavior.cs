using System.Security.Claims;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Authorization;

namespace Budgets.Core;

public abstract record UserBudgetMessage(Guid BudgetId, ClaimsPrincipal User, BudgetPermissions Permissions) : IMessage;

// public class BudgetUsersPipelineBehavior<TMessage>(IAuthorizationService authorizationService): IPipelineBehavior<TMessage, IMaybe>
//     where TMessage : UserBudgetMessage
//
// {
//     public async ValueTask<IMaybe> Handle(
//         TMessage message,
//         MessageHandlerDelegate<TMessage, IMaybe> next,
//         CancellationToken cancellationToken
//     )
//     {
//         if (await authorizationService.CheckUserHasPermission(message.User, message.BudgetId, message.Permissions) is INone
//             permissionError)
//         {
//             return permissionError;
//         }
//         return await next(message, cancellationToken);
//     }
// }

public class BudgetUsersResultedPipelineBehavior<TMessage, TResponse>(IAuthorizationService authorizationService) :
    IPipelineBehavior<TMessage, TResponse> where TMessage : UserBudgetMessage where TResponse : IMaybe
{
    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
    {
        if (await authorizationService.CheckUserHasPermission(message.User, message.BudgetId, message.Permissions))
            return await next(message, cancellationToken);
        var genericArguments = typeof(TResponse).GetGenericArguments();
        var errorMessage = $"User has no permission {message.Permissions}";
            
        if (genericArguments.Length == 0)
        {
            return (TResponse)UserHasNoPermission.Create(errorMessage);
        }

        if (genericArguments.Length != 1)
        {
            throw new InvalidOperationException("Generic IMaybe must have exactly 1 generic argument");
        }
            
        var genericErrorType = typeof(UserHasNoPermission<>).MakeGenericType(genericArguments);
        return (TResponse) Activator.CreateInstance(type: genericErrorType, errorMessage, Array.Empty<NoneDetail>())!;
    }
}