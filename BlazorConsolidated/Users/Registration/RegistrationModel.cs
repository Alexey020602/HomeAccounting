using ClientServerContracts.Users.Register;

namespace BlazorConsolidated.Users.Registration;

internal sealed class RegistrationModel
{
    public string Login { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    internal RegistrationRequest CreateRequest()
    {
        return new RegistrationRequest(Login, Password, FullName);
    }
}