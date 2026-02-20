namespace BlazorConsolidated.Users.Infrastructure.Abstractions;

public interface IAuthenticationStateNotifier
{
    void NotifyAuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task);
}
