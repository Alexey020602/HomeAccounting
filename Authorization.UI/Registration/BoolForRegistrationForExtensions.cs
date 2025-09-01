using MudBlazor;

namespace Authorization.UI.Registration;

static class BoolForRegistrationForExtensions
{
    public static InputType PasswordFieldType(this bool isShow) => isShow 
        ? InputType.Text 
        : InputType.Password;
    public static string PasswordFieldIcon(this bool isShow) => isShow 
        ? Icons.Material.Filled.VisibilityOff 
        :  Icons.Material.Filled.Visibility;
}