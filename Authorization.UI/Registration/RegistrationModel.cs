using Authorization.Contracts;

namespace Authorization.UI.Registration;

internal sealed class RegistrationModel
{
    public string Login { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    internal RegistrationRequest CreateRequest()
    {
        return new RegistrationRequest
        {
            Login = Login,
            Username = UserName,
                
            Password = Password
        };
    }
}